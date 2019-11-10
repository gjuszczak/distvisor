using System;

namespace Distvisor.Web.Data.Models
{
    public class User
    { 
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime LockoutUtc { get; set; }
    }
}
