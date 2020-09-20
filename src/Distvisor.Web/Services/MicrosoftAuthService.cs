using Distvisor.Web.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IMicrosoftAuthService
    {
        Task<OAuthToken> GetUserActiveTokenAsync();
    }

    public class MicrosoftAuthService : IMicrosoftAuthService
    {
        private readonly AzureAdConfiguration _config;
        private readonly IUserInfoProvider _userInfo;
        private readonly RestClient _httpClient;
        private readonly string _requestedScopes;

        public MicrosoftAuthService(IOptions<AzureAdConfiguration> config, IUserInfoProvider userInfo)
        {
            _config = config.Value;
            _userInfo = userInfo;
            _httpClient = new RestClient(_config.Instance);
            _requestedScopes = "User.Read Files.ReadWrite.AppFolder";
        }

        public async Task<OAuthToken> GetUserActiveTokenAsync()
        {
            var accessToken = await _userInfo.GetAccessTokenAsync();

            var request = new RestRequest("consumers/oauth2/v2.0/token", Method.POST);
            request.AddParameter("grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer");
            request.AddParameter("client_id", _config.ClientId);
            request.AddParameter("client_secret", _config.ClientSecret);
            request.AddParameter("assertion", accessToken);
            request.AddParameter("scope", _requestedScopes);
            request.AddParameter("requested_token_use", "on_behalf_of");

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var token = JsonConvert.DeserializeObject<OAuthToken>(response.Content);
            token.Issuer = OAuthTokenIssuer.MicrosoftIdentity;
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

    public enum OAuthTokenIssuer
    {
        MicrosoftIdentity,
    }
}
