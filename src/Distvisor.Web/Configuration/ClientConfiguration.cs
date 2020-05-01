namespace Distvisor.Web.Configuration
{
    public class ClientConfiguration
    {
        public MsalConfiguration Msal { get; set; }
    }

    public class MsalConfiguration
    {
        public AuthMsalConfiguration Auth { get; set; }
    }

    public class AuthMsalConfiguration
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public bool ValidateAuthority { get; set; }
    }
}
