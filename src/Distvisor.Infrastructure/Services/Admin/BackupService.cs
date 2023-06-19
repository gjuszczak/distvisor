using Distvisor.App.Features.Backups.Queries.GetBackupFiles;
using Distvisor.App.Features.Backups.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Services.Admin
{
    public class BackupService : IBackupService
    {
        private readonly IBackupFileService _backupFileService;
        private readonly IFileHostingService _fileHostingService;

        public BackupService(IBackupFileService backupFileService, IFileHostingService fileHostingService)
        {
            _backupFileService = backupFileService;
            _fileHostingService = fileHostingService;
        }

        public async Task CreateAsync(string name, CancellationToken cancellationToken)
        {
            using var tempBackupFile = await _backupFileService.GenerateAsync(cancellationToken);
            await _fileHostingService.UploadAsync(tempBackupFile.Path, name, cancellationToken);
        }

        public async Task RestoreAsync(string name, CancellationToken cancellationToken)
        {
            using var tempBackupFile = await _fileHostingService.DownloadAsync(name, cancellationToken);
            await _backupFileService.RestoreAsync(tempBackupFile.Path, cancellationToken);
        }

        public async Task DeleteAsync(string name, CancellationToken cancellationToken)
            => await _fileHostingService.DeleteAsync(name, cancellationToken);

        public async Task RenameAsync(string oldName, string newName, CancellationToken cancellationToken)
            => await _fileHostingService.RenameAsync(oldName, newName, cancellationToken);

        public async Task<BackupFilesListDto> GetAsync(int offset, int batchSize, CancellationToken cancellationToken)
            => await _fileHostingService.ListAsync(offset, batchSize, cancellationToken);
    }
}
