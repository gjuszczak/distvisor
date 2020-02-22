using Distvisor.Web.Data;
using Distvisor.Web.Data.Entities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IMicrosoftAuthService
    {
        Task<OAuthToken> ExchangeAuthCodeForBearerToken(string authCode);
        string GetAuthorizeUri();
        Task<OAuthToken> RefreshAccessToken(string refreshToken);
    }

    public class MicrosoftAuthService : IMicrosoftAuthService
    {
        private readonly IUserInfoProvider _userInfo;
        private readonly RestClient _httpClient;
        private readonly MicrosoftSecrets _secrets;
        private readonly string _requestedScopes;

        public MicrosoftAuthService(ISecretsVault secretsVault, IUserInfoProvider userInfo)
        {
            _secrets = secretsVault.GetMicrosoftSecrets();
            _userInfo = userInfo;
            _httpClient = new RestClient("https://login.microsoftonline.com/");
            _requestedScopes = "offline_access user.read Files.ReadWrite.AppFolder";
        }

        public string GetAuthorizeUri()
        {
            var request = new RestRequest("consumers/oauth2/v2.0/authorize");
            request.AddQueryParameter("client_id", _secrets.AppClientId);
            request.AddQueryParameter("response_type", "code");
            request.AddQueryParameter("redirect_uri", _secrets.AuthRedirectUri);
            request.AddQueryParameter("response_mode", "query");
            request.AddQueryParameter("scope", _requestedScopes);
            request.AddQueryParameter("state", _userInfo.UserId.ToString());

            var uri = _httpClient.BuildUri(request);
            return uri.ToString();
        }

        public async Task<OAuthToken> ExchangeAuthCodeForBearerToken(string authCode)
        {
            var request = new RestRequest("consumers/oauth2/v2.0/token", Method.POST);
            request.AddParameter("client_id", _secrets.AppClientId);
            request.AddParameter("scope", _requestedScopes);
            request.AddParameter("code", authCode);
            request.AddParameter("redirect_uri", _secrets.AuthRedirectUri);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("client_secret", _secrets.AppSecret);

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var token = JsonConvert.DeserializeObject<OAuthToken>(response.Content);
            token.Issuer = OAuthTokenIssuer.MicrosoftService;
            token.UtcIssueDate = DateTime.UtcNow;
            return token;
        }

        public async Task<OAuthToken> RefreshAccessToken(string refreshToken)
        {
            var request = new RestRequest("consumers/oauth2/v2.0/token", Method.POST);
            request.AddParameter("client_id", _secrets.AppClientId);
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("scope", _requestedScopes);
            request.AddParameter("refresh_token", refreshToken);
            request.AddParameter("redirect_uri", _secrets.AuthRedirectUri);
            request.AddParameter("client_secret", _secrets.AppSecret);

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var token = JsonConvert.DeserializeObject<OAuthToken>(response.Content);
            token.Issuer = OAuthTokenIssuer.MicrosoftService;
            token.UtcIssueDate = DateTime.UtcNow;
            return token;
        }
    }

    public class OAuthToken
    {
        public OAuthTokenIssuer Issuer { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public DateTime UtcIssueDate { get; set; }
    }
}
