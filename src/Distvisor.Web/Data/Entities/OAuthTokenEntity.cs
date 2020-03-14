using System;

namespace Distvisor.Web.Data.Entities
{
    public class OAuthTokenEntity
    {
        public int Id { get; set; }
        public OAuthTokenIssuer Issuer { get; set; }        
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }      
        public string Scope { get; set; }
        public string RefreshToken { get; set; }
        public DateTime UtcIssueDate { get; set; }

        public UserEntity User { get; set; }
    }

    public enum OAuthTokenIssuer
    {
        Distvisor,
        MicrosoftIdentity,
    }
}
