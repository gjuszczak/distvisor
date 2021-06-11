using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IBackupService
    {
        Task<IEnumerable<OneDriveFileInfo>> ListStoredBackupsAsync();
        Task CreateBackupAsync();
        Task RestoreBackupAsync(string fileName);
        Task DeleteBackupAsync(string fileName);
        Task ReplayEventsToReadStoreAsync();
    }

    public class BackupService : IBackupService
    {
        private readonly IOneDriveClient _oneDrive;
        private readonly INotificationService _notifications;
        private readonly IBackupProcessManager _backupProcessManager;

        public BackupService(
            IOneDriveClient oneDrive,
            INotificationService notifications,
            IMicrosoftAuthClient auth,
            IBackupProcessManager backupProcessManager)
        {
            _oneDrive = oneDrive;
            _notifications = notifications;
            _backupProcessManager = backupProcessManager;

            _oneDrive.Configure(async () => (await auth.GetUserActiveTokenAsync()).AccessToken);
        }

        public async Task<IEnumerable<OneDriveFileInfo>> ListStoredBackupsAsync()
        {
            try
            {
                return await _oneDrive.ListStoredBackupsAsync();
            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync("Unable to list stored backups", exc);
                throw;
            }
        }

        public async Task CreateBackupAsync()
        {
            try
            {
                var path = await _backupProcessManager.GenerateBackupFileAsync();
                try
                {
                    var dateString = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    using var session = await _oneDrive.CreateUploadSessionAsync($"/es_db_{dateString}.bak");
                    await session.Upload(path);
                    await _notifications.PushSuccessAsync("Backup created successfully");
                }
                finally
                {
                    File.Delete(path);
                }
            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync("Unable to create backup", exc);
                throw;
            }
        }

        public async Task RestoreBackupAsync(string fileName) 
        {
            try
            {
                var path = await _oneDrive.DownloadFileAsync($"/{fileName}");
                try
                {
                    await _backupProcessManager.RestoreBackupFileAsync(path);
                    await _notifications.PushSuccessAsync("Backup restored successfully");
                }
                finally
                {
                    File.Delete(path);
                }

            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync("Unable to restore backup", exc);
                throw;
            }
        }

        public async Task DeleteBackupAsync(string fileName)
        {
            try
            {
                await _oneDrive.DeleteFileAsync($"/{fileName}");
                await _notifications.PushSuccessAsync("Backup deleted successfully");

            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync("Unable to delete backup", exc);
                throw;
            }
        }

        public async Task ReplayEventsToReadStoreAsync()
        {
            try
            {
                await _backupProcessManager.ReplayEventsToReadStoreAsync();
                await _notifications.PushSuccessAsync("Events replayed successfully");

            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync("Unable to replay events", exc);
                throw;
            }
        }
    }
}
