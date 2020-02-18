using Distvisor.Web.Data;
using Distvisor.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IMicrosoftService
    {
        Task<MicrosoftToken> ExchangeAuthCodeForBearerToken(string authCode);
        string GetAuthorizeUri(string state);
        Task StoreUserToken(Guid userId, MicrosoftToken token);
    }

    public class MicrosoftService : IMicrosoftService
    {
        private readonly RestClient _httpClient;
        private readonly MicrosoftSecrets _secrets;
        private readonly DistvisorContext _context;
        private readonly string _requestedScopes;

        public MicrosoftService(ISecretsVault secretsVault, DistvisorContext context)
        {
            _secrets = secretsVault.GetMicrosoftSecrets();
            _context = context;
            _httpClient = new RestClient("https://login.microsoftonline.com/");
            _requestedScopes = "offline_access user.read";
        }

        public string GetAuthorizeUri(string state)
        {
            var request = new RestRequest("consumers/oauth2/v2.0/authorize");
            request.AddQueryParameter("client_id", _secrets.AppClientId);
            request.AddQueryParameter("response_type", "code");
            request.AddQueryParameter("redirect_uri", _secrets.AuthRedirectUri);
            request.AddQueryParameter("response_mode", "query");
            request.AddQueryParameter("scope", _requestedScopes);
            request.AddQueryParameter("state", state);

            var uri = _httpClient.BuildUri(request);
            return uri.ToString();
        }

        public async Task<MicrosoftToken> ExchangeAuthCodeForBearerToken(string authCode)
        {
            var request = new RestRequest("consumers/oauth2/v2.0/token", Method.POST);
            request.AddParameter("client_id", _secrets.AppClientId);
            request.AddParameter("scope", _requestedScopes);
            request.AddParameter("code", authCode);
            request.AddParameter("redirect_uri", _secrets.AuthRedirectUri);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("client_secret", _secrets.AppSecret);

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var token = JsonConvert.DeserializeObject<MicrosoftToken>(response.Content);
            return token;
        }

        public async Task StoreUserToken(Guid userId, MicrosoftToken token)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to store user token. User with id = {userId} does not exists.");
            }

            var tokenEntity = await _context.OAuthTokens
                .Where(x => x.User == user && x.Issuer == OAuthTokenIssuer.MicrosoftService)
                .FirstOrDefaultAsync();

            if (tokenEntity == null)
            {
                tokenEntity = new OAuthTokenEntity();
                tokenEntity.User = user;
                _context.OAuthTokens.Add(tokenEntity);
            }

            tokenEntity.AccessToken = token.AccessToken;
            tokenEntity.TokenType = token.TokenType;
            tokenEntity.ExpiresIn = DateTime.Now.AddSeconds(token.ExpiresIn);
            tokenEntity.Scope = token.Scope;
            tokenEntity.RefreshToken = token.RefreshToken;

            await _context.SaveChangesAsync();
        }
    }

    public class MicrosoftToken
    {
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
    }
}
