using Distvisor.App.Features.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Services.Admin
{
    public class FileHostingAuthService : IFileHostingAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ICurrentUserService _userService;
        private readonly FileHostingAuthConfiguration _config;
        private readonly IMemoryCache _memoryCache;

        public FileHostingAuthService(HttpClient httpClient, ICurrentUserService userService, IOptions<FileHostingAuthConfiguration> config, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _userService = userService;
            _config = config.Value;
            _memoryCache = memoryCache;
        }

        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            var cacheKey = $"{nameof(FileHostingAuthService)}_{_userService.UserId}";
            if (_memoryCache.TryGetValue(cacheKey, out object value) && value is string cachedAccessToken)
            {
                return cachedAccessToken;
            }

            var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"consumers/oauth2/v2.0/token",
                Port = -1
            };

            var userAccessToken = await _userService.GetAccessTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, urlBuilder.ToString())
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "urn:ietf:params:oauth:grant-type:jwt-bearer",
                    ["client_id"] = _config.ClientId,
                    ["client_secret"] = _config.ClientSecret,
                    ["assertion"] = userAccessToken,
                    ["scope"] = "User.Read Files.ReadWrite.AppFolder",
                    ["requested_token_use"] = "on_behalf_of",
                })
            };

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var token = await JsonSerializer.DeserializeAsync<JsonDocument>(stream, cancellationToken: cancellationToken);

            var accessToken = token.RootElement.GetProperty("access_token").GetString();
            var expiresIn = token.RootElement.GetProperty("expires_in").GetInt32();

            var cacheItemExpiration = DateTimeOffset.UtcNow.AddSeconds(expiresIn).AddMinutes(-5);
            _memoryCache.Set(cacheKey, accessToken, cacheItemExpiration);

            return accessToken;
        }
    }
}
