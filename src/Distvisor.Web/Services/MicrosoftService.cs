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
        Task CreateUploadSession(string filename);
        Task<MicrosoftToken> ExchangeAuthCodeForBearerToken(string authCode);
        string GetAuthorizeUri();
        Task<MicrosoftToken> GetUserActiveToken();
        Task StoreUserToken(Guid userId, MicrosoftToken token);
        Task StoreUserToken(MicrosoftToken token);
    }

    public class MicrosoftService : IMicrosoftService
    {
        private readonly IUserInfoProvider _userInfo;
        private readonly RestClient _httpIdentityClient;
        private readonly RestClient _httpGraphClient;
        private readonly MicrosoftSecrets _secrets;
        private readonly DistvisorContext _context;
        private readonly string _requestedScopes;

        public MicrosoftService(ISecretsVault secretsVault, IUserInfoProvider userInfo, DistvisorContext context)
        {
            _secrets = secretsVault.GetMicrosoftSecrets();
            _userInfo = userInfo;
            _context = context;
            _httpIdentityClient = new RestClient("https://login.microsoftonline.com/");
            _httpGraphClient = new RestClient("https://graph.microsoft.com/");
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

            var uri = _httpIdentityClient.BuildUri(request);
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

            var response = await _httpIdentityClient.ExecuteAsync(request, CancellationToken.None);
            var token = JsonConvert.DeserializeObject<MicrosoftToken>(response.Content);
            return token;
        }

        public async Task<MicrosoftToken> RefreshAccessToken(MicrosoftToken oldToken)
        {
            var request = new RestRequest("consumers/oauth2/v2.0/token", Method.POST);
            request.AddParameter("client_id", _secrets.AppClientId);
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("scope", _requestedScopes);
            request.AddParameter("refresh_token", oldToken.RefreshToken);
            request.AddParameter("redirect_uri", _secrets.AuthRedirectUri);
            request.AddParameter("client_secret", _secrets.AppSecret);

            var response = await _httpIdentityClient.ExecuteAsync(request, CancellationToken.None);
            var token = JsonConvert.DeserializeObject<MicrosoftToken>(response.Content);
            return token;
        }

        public async Task CreateUploadSession(string filename)
        {
            var token = await GetUserActiveToken();
            
            // make sure that approot is created before upload session request
            var request = new RestRequest($"v1.0/me/drive/special/approot", Method.GET);
            request.AddHeader("Authorization", "Bearer " + token.AccessToken);
            var response = await _httpGraphClient.ExecuteAsync(request, CancellationToken.None);

            request = new RestRequest($"v1.0/me/drive/special/approot:{filename}:/createUploadSession", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token.AccessToken);
            //request.AddFile("scheme.txt", @"S:\priv\color_scheme.txt", "text/plain");

            response = await _httpGraphClient.ExecuteAsync(request, CancellationToken.None);
        }

        public Task StoreUserToken(MicrosoftToken token)
        {
            return StoreUserToken(_userInfo.UserId, token);
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

        public async Task<MicrosoftToken> GetUserActiveToken()
        {
            var user = await _context.Users.FindAsync(_userInfo.UserId);
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to get user token. User with id = {_userInfo.UserId} does not exists.");
            }

            var tokenEntity = await _context.OAuthTokens
              .Where(x => x.User == user && x.Issuer == OAuthTokenIssuer.MicrosoftService)
              .FirstOrDefaultAsync();

            if (tokenEntity == null)
            {
                return null;
            }

            var token = new MicrosoftToken
            {
                AccessToken = tokenEntity.AccessToken,
                ExpiresIn = 0,
                RefreshToken = tokenEntity.RefreshToken,
                Scope = tokenEntity.Scope,
                TokenType = tokenEntity.TokenType,
            };

            if (tokenEntity.ExpiresIn > DateTime.Now)
            {
                return token;
            }

            var refreshedToken = await RefreshAccessToken(token);
            await StoreUserToken(refreshedToken);
            return refreshedToken;
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
