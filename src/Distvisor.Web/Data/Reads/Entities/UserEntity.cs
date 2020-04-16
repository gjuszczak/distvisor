using System;

namespace Distvisor.Web.Data.Entities
{
    public class UserEntity
    { 
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime LockoutUtc { get; set; }
    }
}
