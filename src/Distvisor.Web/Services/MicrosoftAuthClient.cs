using Distvisor.Web.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IMicrosoftAuthClient
    {
        Task<OAuthToken> GetUserActiveTokenAsync();
    }

    public class MicrosoftAuthClient : IMicrosoftAuthClient
    {
        private readonly AzureAdConfiguration _config;
        private readonly IUserInfoProvider _userInfo;
        private readonly HttpClient _httpClient;
        private readonly string _requestedScopes;

        public MicrosoftAuthClient(HttpClient httpClient, IOptions<AzureAdConfiguration> config, IUserInfoProvider userInfo)
        {
            _config = config.Value;
            _userInfo = userInfo;
            _httpClient = httpClient;
            _requestedScopes = "User.Read Files.ReadWrite.AppFolder";
        }

        public async Task<OAuthToken> GetUserActiveTokenAsync()
        {
            var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"consumers/oauth2/v2.0/token",
                Port = -1
            };

            var accessToken = await _userInfo.GetAccessTokenAsync();

            var formUrlEncodedContent = new Dictionary<string, string>
            {
                ["grant_type"] = "urn:ietf:params:oauth:grant-type:jwt-bearer",
                ["client_id"] = _config.ClientId,
                ["client_secret"] = _config.ClientSecret,
                ["assertion"] = accessToken,
                ["scope"] = _requestedScopes,
                ["requested_token_use"] = "on_behalf_of",
            };

            var request = new HttpRequestMessage(HttpMethod.Post, urlBuilder.ToString());
            request.Content = new FormUrlEncodedContent(formUrlEncodedContent);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            using var stream = await response.Content.ReadAsStreamAsync();
            var token = await JsonSerializer.DeserializeAsync<OAuthToken>(stream);
            token.Issuer = OAuthTokenIssuer.MicrosoftIdentity;
            token.UtcIssueDate = DateTime.UtcNow;
            return token;
        }
    }

    public class OAuthToken
    {
        public OAuthTokenIssuer Issuer { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        public DateTime UtcIssueDate { get; set; }
    }

    public enum OAuthTokenIssuer
    {
        MicrosoftIdentity,
    }
}
