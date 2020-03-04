using Distvisor.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface INotificationService
    {
        Task PushSuccessAsync(string message);
    }

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationsHub, INotificationClient> _notificationsHub;
        private readonly IUserInfoProvider _userInfo;

        public NotificationService(
            IHubContext<NotificationsHub, INotificationClient> notifictionsHub,
            IUserInfoProvider userInfo)
        {
            _notificationsHub = notifictionsHub;
            _userInfo = userInfo;
        }

        public async Task PushSuccessAsync(string message)
        {
            await _notificationsHub.Clients.User(_userInfo.UserId.ToString()).PushNotification($"Success: {message}");
        }
    }
}
