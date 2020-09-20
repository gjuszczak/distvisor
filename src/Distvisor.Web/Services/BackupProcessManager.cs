using Distvisor.Web.Data;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Events.Entities;
using Distvisor.Web.Data.Reads.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IBackupProcessManager
    {
        Task<string> GenerateBackupFileAsync();
        Task RestoreBackupFileAsync(string filePath);
        Task ReplayEventsToReadStoreAsync();
    }

    public class BackupProcessManager : IBackupProcessManager
    {
        private const int EventsBatchSize = 1000;
        private readonly IEventStore _es;
        private readonly EventStoreContext _esContext;
        private readonly ReadStoreContext _rsContext;

        public BackupProcessManager(IEventStore es, EventStoreContext esContext, ReadStoreContext rsContext)
        {
            _es = es;
            _esContext = esContext;
            _rsContext = rsContext;
        }

        public async Task<string> GenerateBackupFileAsync()
        {
            var tempDirPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirPath);
            try
            {
                var fileNo = 0;
                await foreach (var eventBatch in GetAllDbEventsInBatchesAsync())
                {
                    using FileStream fs = File.Create(Path.Combine(tempDirPath, $"{fileNo}.json"));
                    await JsonSerializer.SerializeAsync(fs, eventBatch, JsonDocumentConverter.Options);
                    fileNo += EventsBatchSize;
                }
                var zipFileTempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                ZipFile.CreateFromDirectory(tempDirPath, zipFileTempPath);
                return zipFileTempPath;
            }
            finally
            {
                Directory.Delete(tempDirPath, true);
            }
        }

        public async Task RestoreBackupFileAsync(string filePath)
        {
            var tempDirPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            ZipFile.ExtractToDirectory(filePath, tempDirPath);
            try
            {
                var backupContentFiles = Directory.GetFiles(tempDirPath)
                    .Select<string, (string path, int? fileNo)>(x => int.TryParse(Path.GetFileNameWithoutExtension(x), out int fileNo) ? (x, fileNo) : (x, (int?)null))
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

                using var transaction = await _esContext.Database.BeginTransactionAsync();
                await _esContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Events\";");
                foreach ((var contentFilePath, var _) in backupContentFiles)
                {
                    using FileStream fs = File.OpenRead(contentFilePath);
                    var events = await JsonSerializer.DeserializeAsync<List<EventEntity>>(fs, JsonDocumentConverter.Options);
                    events.ForEach(x => x.Id = 0);
                    _esContext.Events.AddRange(events);
                    await _esContext.SaveChangesAsync();
                    _esContext.DetachAllEntities();
                };
                await transaction.CommitAsync();
            }
            finally
            {
                Directory.Delete(tempDirPath, true);
            }
        }


        public async Task ReplayEventsToReadStoreAsync()
        {
            var tables = _rsContext.Model.GetEntityTypes().Select(x => x.GetTableName());
            using var transaction = await _rsContext.Database.BeginTransactionAsync();
            foreach (var table in tables)
            {
                await _rsContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE \"{table}\" CASCADE;");
            }
            await _es.ReplayEvents();
            await transaction.CommitAsync();
        }

        private async IAsyncEnumerable<IEnumerable<EventEntity>> GetAllDbEventsInBatchesAsync()
        {
            var eventsCount = await _esContext.Events.CountAsync();
            for (int i = 0; i < eventsCount; i += EventsBatchSize)
            {
                yield return await _esContext.Events
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .Skip(i)
                    .Take(EventsBatchSize)
                    .ToListAsync();
            }
        }

        private sealed class JsonDocumentConverter : JsonConverter<JsonDocument>
        {
            public override JsonDocument Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return JsonDocument.ParseValue(ref reader);
            }

            public override void Write(Utf8JsonWriter writer, JsonDocument value, JsonSerializerOptions options)
            {
                value.WriteTo(writer);
            }

            private static Lazy<JsonSerializerOptions> options = new Lazy<JsonSerializerOptions>(() =>
            {
                var opt = new JsonSerializerOptions();
                opt.Converters.Add(new JsonDocumentConverter());
                return opt;
            });

            public static JsonSerializerOptions Options => options.Value;
        }
    }
}
