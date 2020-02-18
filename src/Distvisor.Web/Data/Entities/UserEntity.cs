using System;
using System.Collections.Generic;

namespace Distvisor.Web.Data.Entities
{
    public class UserEntity
    { 
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime LockoutUtc { get; set; }
        public string SessionId { get; set; }
        public DateTime SessionStartUtc { get; set; }
        public DateTime SessionExpirationUtc { get; set; }

        public List<OAuthTokenEntity> OAuthTokens { get; set; } = new List<OAuthTokenEntity>();
    }
}
