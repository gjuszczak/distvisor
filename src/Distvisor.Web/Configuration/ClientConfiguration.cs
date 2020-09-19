namespace Distvisor.Web.Configuration
{
    public class ClientConfiguration
    {
        public MsalConfiguration Msal { get; set; }
        public MsalAngularConfiguration MsalAngular { get; set; }
        public BackendDetails BackendDetails { get; set; }
    }

    public class BackendDetails
    {
        public string AppVersion { get; set; }
        public string RuntimeVersion { get; set; }
        public string Environment { get; set; }
    }

    public class MsalConfiguration
    {
        public AuthMsalConfiguration Auth { get; set; }
        public CacheMsalConfiguration Cache { get; set; }
    }

    public class MsalAngularConfiguration
    {
        public string[] ConsentScopes { get; set; }
    }

    public class AuthMsalConfiguration
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public bool ValidateAuthority { get; set; }
    }

    public class CacheMsalConfiguration
    {
        public string CacheLocation { get; set; }
    }
}
