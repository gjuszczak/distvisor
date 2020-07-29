using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IBackupService
    {
        Task<IEnumerable<OneDriveFileInfo>> ListStoredBackups();
    }

    public class BackupService : IBackupService
    {
        private readonly IOneDriveClient _oneDrive;
        private readonly INotificationService _notifications;

        public BackupService(IOneDriveClient oneDrive, INotificationService notifications, IMicrosoftAuthService auth)
        {
            _oneDrive = oneDrive;
            _notifications = notifications;

            _oneDrive.Configure(async () => (await auth.GetUserActiveTokenAsync()).AccessToken);
        }

        public async Task<IEnumerable<OneDriveFileInfo>> ListStoredBackups()
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
    }
}
