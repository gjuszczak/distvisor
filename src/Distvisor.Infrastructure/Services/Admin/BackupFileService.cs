using System.IO.Compression;
using System.IO;
using System.Text.Json;
using Distvisor.App.Core.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Distvisor.Infrastructure.Persistence.Events;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;
using System;

namespace Distvisor.Infrastructure.Services.Admin
{
    public class BackupFileService : IBackupFileService
    {
        private const int EventsBatchSize = 1000;

        private readonly EventsDbContext _eventsDbContext;

        public BackupFileService(EventsDbContext eventsDbContext)
        {
            _eventsDbContext = eventsDbContext;
        }

        public async Task<TemporaryPath> GenerateAsync(CancellationToken cancellationToken)
        {
            using var tempDir = TemporaryPath.ForDirectory();
            Directory.CreateDirectory(tempDir.Path);

            var fileNo = 0;
            await foreach (var eventBatch in GetAllEventsInBatchesAsync(cancellationToken))
            {
                using FileStream fs = File.Create(Path.Combine(tempDir.Path, $"{fileNo}.json"));
                await JsonSerializer.SerializeAsync(fs, eventBatch, cancellationToken: cancellationToken);
                fileNo += EventsBatchSize;
            }
            var tempZipFile = TemporaryPath.ForFile();
            ZipFile.CreateFromDirectory(tempDir.Path, tempZipFile.Path);
            return tempZipFile;
        }

        public async Task RestoreAsync(string filePath, CancellationToken cancellationToken)
        {
            using var tempDir = TemporaryPath.ForDirectory();
            ZipFile.ExtractToDirectory(filePath, tempDir.Path);

            var backupContentFiles = Directory.GetFiles(tempDir.Path)
                .Select<string, (string path, int? fileNo)>(x => int.TryParse(Path.GetFileNameWithoutExtension(x), out int fileNo) ? (x, fileNo) : (x, null))
                .OrderBy(x => x.fileNo)
                .ToList();

            if (backupContentFiles.Any(x => !x.fileNo.HasValue))
            {
                throw new Exception("Backup content files validation failed. Invalid content file names.");
            }

            if (backupContentFiles.Any(x => Path.GetExtension(x.path).ToUpper() != ".JSON"))
            {
                throw new Exception("Backup content files validation failed. Invalid content file extensions.");
            }

            var eventEntityModelType = _eventsDbContext.Model.FindEntityType(typeof(EventEntity));
            var schema = eventEntityModelType.GetSchema();
            var table = eventEntityModelType.GetTableName();
            var identityColumn = eventEntityModelType.GetProperties()
                .Where(x => x.Name == nameof(EventEntity.EventId))
                .Select(x => x.GetColumnName())
                .Single();

            using var transaction = await _eventsDbContext.Database.BeginTransactionAsync(cancellationToken);
            await _eventsDbContext.Database.ExecuteSqlRawAsync($"TRUNCATE \"{schema}\".\"{table}\" RESTART IDENTITY;", cancellationToken);
            foreach ((var contentFilePath, var _) in backupContentFiles)
            {
                using FileStream fs = File.OpenRead(contentFilePath);
                var events = await JsonSerializer.DeserializeAsync<EventEntity[]>(fs, cancellationToken: cancellationToken);
                _eventsDbContext.Events.AddRange(events);
                await _eventsDbContext.SaveChangesAsync(cancellationToken);
                DetachAllEntities(_eventsDbContext);
            };
            await _eventsDbContext.Database.ExecuteSqlRawAsync($"SELECT SETVAL('\"{schema}\".\"{table}_{identityColumn}_seq\"', (SELECT MAX(\"{identityColumn}\") FROM \"{schema}\".\"{table}\"));", cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }

        private async IAsyncEnumerable<IEnumerable<EventEntity>> GetAllEventsInBatchesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var eventOffsetId = 0L;
            while (true)
            {
                var events = await _eventsDbContext.Events
                    .AsNoTracking()
                    .Where(x => x.EventId > eventOffsetId)
                    .OrderBy(x => x.EventId)
                    .Take(EventsBatchSize)
                    .ToListAsync(cancellationToken);

                if (!events.Any())
                {
                    break;
                }

                eventOffsetId = events.Last().EventId;
                yield return events;
            }
        }

        private static void DetachAllEntities(DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            foreach (var entry in entries.Where(x => x.Entity != null))
            {
                entry.State = EntityState.Detached;
            }
        }
    }
}
