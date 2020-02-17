namespace Distvisor.Web.Services
{
    public interface IMicrosoftService
    {
        string GetAuthorizeUrl();
    }

    public class MicrosoftService : IMicrosoftService
    {
        private MicrosoftSecrets _secrets;

        public MicrosoftService(ISecretsVault secretsVault)
        {
            _secrets = secretsVault.GetMicrosoftSecrets();
        }

        public string GetAuthorizeUrl()
        {
            return $"https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize" +
                $"?client_id={_secrets.AppClientId}" +
                $"&response_type=code" +
                $"&redirect_uri={UrlEncode(_secrets.AuthRedirectUri)}" +
                $"&response_mode=query" +
                $"&scope=user.read";
        }

        private string UrlEncode(string text)
        {
            return System.Net.WebUtility.UrlEncode(text);
        }
    }
}
