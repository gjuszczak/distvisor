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
        Task<OAuthToken> ExchangeAuthCodeForBearerTokenAsync(string authCode, Guid userId);
        string GetAuthorizeUri();
        Task<OAuthToken> GetUserActiveTokenAsync();
    }

    public class MicrosoftAuthService : IMicrosoftAuthService
    {
        private readonly IUserInfoProvider _userInfo;
        private readonly IAuthTokenStore _tokenStore;
        private readonly RestClient _httpClient;
        private readonly MicrosoftSecrets _secrets;
        private readonly string _requestedScopes;

        public MicrosoftAuthService(ISecretsVault secretsVault, IUserInfoProvider userInfo, IAuthTokenStore tokenStore)
        {
            _secrets = secretsVault.GetMicrosoftSecrets();
            _userInfo = userInfo;
            _tokenStore = tokenStore;
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

        public async Task<OAuthToken> ExchangeAuthCodeForBearerTokenAsync(string authCode, Guid userId)
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
            token.Issuer = OAuthTokenIssuer.MicrosoftIdentity;
            token.UtcIssueDate = DateTime.UtcNow;

            await _tokenStore.StoreUserTokenAsync(token, userId);
            return token;
        }

        public async Task<OAuthToken> GetUserActiveTokenAsync()
        {
            var token = await _tokenStore.GetUserStoredTokenAsync(OAuthTokenIssuer.MicrosoftIdentity, _userInfo.UserId);
            var tokenExpiration = token.UtcIssueDate
                .ToLocalTime()
                .AddSeconds(token.ExpiresIn)
                .AddMinutes(-1);

            if (tokenExpiration > DateTime.Now)
            {
                return token;
            }

            var refreshedToken = await RefreshAccessTokenAsync(token.RefreshToken);
            return refreshedToken;
        }

        private async Task<OAuthToken> RefreshAccessTokenAsync(string refreshToken)
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
            token.Issuer = OAuthTokenIssuer.MicrosoftIdentity;
            token.UtcIssueDate = DateTime.UtcNow;

            await _tokenStore.StoreUserTokenAsync(token, _userInfo.UserId);
            return token;
        }
    }
}
