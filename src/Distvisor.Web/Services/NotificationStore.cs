using Distvisor.Web.Data;
using Distvisor.Web.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface INotificationStore
    {
        Task StoreNotificationAsync(Notification notification);
    }

    public class NotificationStore : INotificationStore
    {
        private readonly DistvisorContext _context;
        private readonly IUserInfoProvider _userInfo;

        public NotificationStore(DistvisorContext context, IUserInfoProvider userInfo)
        {
            _context = context;
            _userInfo = userInfo;
        }

        public async Task StoreNotificationAsync(Notification notification)
        {
            var user = await _context.Users.FindAsync(_userInfo.UserId);
            user.Notifications.Add(notification.ToEntity());
            await _context.SaveChangesAsync();
        }
    }

    public abstract class Notification
    {
        public Notification()
        {
            GenerationDate = DateTime.Now;
        }

        public DateTime GenerationDate { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });
        }

        public NotificationEntity ToEntity()
        {
            return new NotificationEntity
            {
                UtcGeneratedDate = GenerationDate.ToUniversalTime(),
                Payload = this.ToJsonString(),
            };
        }

        public static Notification FromEntity(NotificationEntity entity)
        {
            var notification = JsonConvert.DeserializeObject<Notification>(entity.Payload, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });
            notification.GenerationDate = entity.UtcGeneratedDate.ToLocalTime();
            return notification;
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
}
