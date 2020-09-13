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
        Task DeleteBackupAsync(string fileName);
    }

    public class BackupService : IBackupService
    {
        private readonly IOneDriveClient _oneDrive;
        private readonly INotificationService _notifications;
        private readonly IBackupFilesManager _backupFilesManager;

        public BackupService(
            IOneDriveClient oneDrive,
            INotificationService notifications,
            IMicrosoftAuthService auth,
            IBackupFilesManager backupFilesManager)
        {
            _oneDrive = oneDrive;
            _notifications = notifications;
            _backupFilesManager = backupFilesManager;

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
                var path = await _backupFilesManager.GenerateBackupFileAsync();
                try
                {
                    var dateString = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    using var session = await _oneDrive.CreateUploadSession($"/es_db_{dateString}.bak");
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

        public async Task DeleteBackupAsync(string fileName)
        {
            try
            {
                await _oneDrive.DeleteFile($"/{fileName}");
                await _notifications.PushSuccessAsync("Backup deleted successfully");

            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync("Unable to delete backup", exc);
                throw;
            }
        }
    }
}
