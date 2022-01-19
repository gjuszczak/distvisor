using Distvisor.App.HomeBox.Services.Gateway;
using Distvisor.App.HomeBox.ValueObjects;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
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

        public async Task<GatewayAuthenticationResponse> LoginAsync(string username, string password)
        {
            return new GatewayAuthenticationResponse
            {
                Token = new GatewayToken("at", "rt", DateTimeOffset.Now)
            };

            var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"api/user/login"
            };

            var body = JsonSerializer.Serialize(new
            {
                email = username,
                password = password,
                appid = _config.AppId,
                nonce = GatewayHelper.GenerateNonce(),
                ts = GatewayHelper.GenerateTimestamp(),
                version = GatewayHelper.Constants.VERSION,
            });

            var url = urlBuilder.ToString();

            var signature = GatewayHelper.HmacSha256Base64(body, _config.AppSecret);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Sign", signature);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GatewayLoginResultDto>(responseContent);
            return new GatewayAuthenticationResponse
            {
                Token = new GatewayToken(result.at, result.rt, DateTimeOffset.Now)
            };
        }

        public Task<GatewayAuthenticationResponse> RefreshSessionAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }

    public class GatewayLoginResultDto
    {
        public string at;
        public string rt;
    }
}
