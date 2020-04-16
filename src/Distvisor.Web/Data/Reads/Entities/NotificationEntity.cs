using System;

namespace Distvisor.Web.Data.Entities
{
    public class NotificationEntity
    {
        public Guid Id { get; set; }
        public DateTime UtcGeneratedDate { get; set; }
        public string Payload { get; set; }

        public UserEntity User { get; set; }
    }

}
