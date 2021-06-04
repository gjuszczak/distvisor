namespace Distvisor.Web.Configuration
{
    public class ClientConfiguration
    {
        public MsalConfiguration Msal { get; set; }
        public MsalInterceptorConfiguration MsalInterceptor { get; set; }
        public MsalGuardConfiguration MsalGuard { get; set; }
        public MsalProtectedResource[] MsalProtectedResources { get; set; }
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

    public class MsalInterceptorConfiguration
    {
        public string InteractionType { get; set; }
    }

    public class MsalGuardConfiguration
    {
        public string InteractionType { get; set; }
        public string LoginFailedRoute { get; set; }
    }

    public class MsalProtectedResource
    {
        public string[] Urls { get; set; }
        public string[] Scopes { get; set; }
    }

    public class AuthMsalConfiguration
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string PostLogoutRedirectUri { get; set; }
    }

    public class CacheMsalConfiguration
    {
        public string CacheLocation { get; set; }
    }
}
