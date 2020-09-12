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
                    await _backupFilesManager.RestoreBackupFileAsync(path);
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
    }
}
