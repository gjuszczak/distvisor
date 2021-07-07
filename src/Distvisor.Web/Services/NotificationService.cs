using Distvisor.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface INotificationService
    {
        Task PushErrorAsync(string message, Exception exception = null);
        Task PushFakeApiUsedAsync(string api, object requestParams);
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
            var notification = new SuccessNotification { Message = message };
            await PushAsync(notification);
        }

        public async Task PushErrorAsync(string message, Exception exception = null)
        {
            var notification = new ErrorNotification
            {
                Message = message,
                ExceptionMessage = exception?.Message,
                ExceptionDetails = exception?.ToString()
            };
            await PushAsync(notification);
        }

        public async Task PushFakeApiUsedAsync(string api, object requestParams)
        {
            var notification = new FakeApiUsedNotification
            {
                Api = api,
                RequestParams = requestParams,
            };
            await PushAsync(notification);
        }

        private async Task PushAsync(Notification notification)
        {
            await _notificationsHub.Clients
                .User(_userInfo.UserId)
                .PushNotification(notification.ToJsonString());
        }
    }

    public abstract class Notification
    {
        public Notification()
        {
            GenerationDate = DateTime.Now;
        }

        public string TypeName { get => GetType().Name; }
        public DateTime GenerationDate { get; set; }

        public string ToJsonString()
        {
            var json = JsonSerializer.Serialize(this, GetType(), new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreReadOnlyProperties = false,
            });
            return json;
        }
    }

    public class SuccessNotification : Notification
    {
        public string Message { get; set; }
    }

    public class ErrorNotification : Notification
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionDetails { get; set; }
    }

    public class FakeApiUsedNotification : Notification
    {
        public string Api { get; set; }
        public object RequestParams { get; set; }
    }
}
