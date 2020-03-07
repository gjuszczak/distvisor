using Distvisor.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface INotificationService
    {
        Task PushErrorAsync(string message, Exception exception = null);
        Task PushSuccessAsync(string message);
    }

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationsHub, INotificationClient> _notificationsHub;
        private readonly INotificationStore _notificationStore;
        private readonly IUserInfoProvider _userInfo;

        public NotificationService(
            IHubContext<NotificationsHub, INotificationClient> notifictionsHub,
            INotificationStore notificationStore,
            IUserInfoProvider userInfo)
        {
            _notificationsHub = notifictionsHub;
            _notificationStore = notificationStore;
            _userInfo = userInfo;
        }

        public async Task PushSuccessAsync(string message)
        {
            var notification = new SuccessNotification { Message = message };
            await StoreAndPushAsync(notification);
        }

        public async Task PushErrorAsync(string message, Exception exception = null)
        {
            var notification = new ErrorNotification
            {
                Message = message,
                ExceptionMessage = exception?.Message,
                ExceptionDetails = exception?.ToString()
            };
            await StoreAndPushAsync(notification);
        }

        private async Task StoreAndPushAsync(Notification notification)
        {
            await _notificationStore.StoreNotificationAsync(notification);
            await _notificationsHub.Clients.User(_userInfo.UserId.ToString()).PushNotification(notification.ToJsonString());
        }
    }
}
