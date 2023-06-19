using Distvisor.App.Features.HomeBox.Services.Gateway;
using Distvisor.App.Features.HomeBox.ValueObjects;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Services.HomeBox
{
    public class GatewayAuthenticationClient : IGatewayAuthenticationClient
    {
        private readonly HttpClient _httpClient;
        private readonly GatewayConfiguration _config;

        public GatewayAuthenticationClient(HttpClient client, IOptions<GatewayConfiguration> config)
        {
            _httpClient = client;
            _config = config.Value;
        }

        public async Task<GatewayAuthenticationResult> LoginAsync(string username, string password)
        {
            var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"/v2/user/login"
            };

            var body = JsonSerializer.Serialize(new
            {
                email = username,
                password = password,
                appid = _config.AppId,
                countryCode = _config.CountryCode,
            });

            var url = urlBuilder.ToString();

            var signature = GatewayHelper.HmacSha256Base64(body, _config.AppSecret);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Sign", signature);
            request.Headers.Add("X-CK-Appid", _config.AppId);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var streamContent = await response.Content.ReadAsStreamAsync();
            var result = JsonSerializer.Deserialize<GatewayResponseDto<GatewayLoginResponseData>>(streamContent);
            return new GatewayAuthenticationResult
            {
                Token = new GatewayToken(result.Data.AccessToken, result.Data.RefreshToken, DateTimeOffset.UtcNow)
            };
        }

        public async Task<GatewayAuthenticationResult> RefreshSessionAsync(string refreshToken)
        {
            var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"/v2/user/refresh"
            };

            var body = JsonSerializer.Serialize(new
            {
                rt = refreshToken,
            });

            var url = urlBuilder.ToString();

            var signature = GatewayHelper.HmacSha256Base64(body, _config.AppSecret);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Sign", signature);
            request.Headers.Add("X-CK-Appid", _config.AppId);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var streamContent = await response.Content.ReadAsStreamAsync();
            var result = JsonSerializer.Deserialize<GatewayResponseDto<GatewayLoginResponseData>>(streamContent);
            return new GatewayAuthenticationResult
            {
                Token = new GatewayToken(result.Data.AccessToken, result.Data.RefreshToken, DateTimeOffset.UtcNow)
            };
        }
    }

    public class GatewayResponseDto<T>
    {
        [JsonPropertyName("error")]
        public int Error { get; set; }

        [JsonPropertyName("msg")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }
    }

    public class GatewayLoginResponseData
    {
        [JsonPropertyName("at")]
        public string AccessToken { get; init; }

        [JsonPropertyName("rt")]
        public string RefreshToken { get; init; }
    }
}
