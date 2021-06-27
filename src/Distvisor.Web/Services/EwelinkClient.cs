using Distvisor.Web.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IEwelinkClient
    {
        Task<EwelinkDtoDeviceList> GetDevices();
        Task SetDeviceParams(string deviceId, object parameters);
    }

    public class EwelinkClient : IEwelinkClient, ITokenProvider
    {
        private readonly HttpClient _httpClient;
        private readonly EwelinkConfiguration _config;
        private readonly ICryptoService _cryptoService;
        private readonly ITokenCacheManager _tokenCacheManager;

        public EwelinkClient(HttpClient client,
            IOptions<EwelinkConfiguration> config,
            ICryptoService cryptoService,
            ITokenCacheManager tokenCacheManager)
        {
            _httpClient = client;
            _config = config.Value;
            _cryptoService = cryptoService;
            _tokenCacheManager = tokenCacheManager;
        }

        public async Task<EwelinkDtoDeviceList> GetDevices()
        {
            return await _tokenCacheManager.RetryWithToken(this, async (accessToken, _) =>
            {
                var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
                {
                    Path = $"api/user/device"
                };

                var url = QueryHelpers.AddQueryString(urlBuilder.ToString(), new Dictionary<string, string>
                {
                    ["lang"] = "en",
                    ["getTags"] = "1",
                    ["version"] = EwelinkHelper.Constants.APP_VERSION,
                    ["ts"] = EwelinkHelper.GenerateTimestamp(),
                    ["appid"] = EwelinkHelper.Constants.APP_ID,
                    ["imei"] = EwelinkHelper.GenerateFakeImei(),
                    ["os"] = EwelinkHelper.Constants.OS,
                    ["model"] = EwelinkHelper.Constants.MODEL,
                    ["romVersion"] = EwelinkHelper.Constants.ROM_VERSION,
                    ["appVersion"] = EwelinkHelper.Constants.APP_VERSION,
                });

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<EwelinkDtoDeviceList>(responseContent);
                return result;
            });
        }

        public async Task SetDeviceParams(string deviceId, object parameters)
        {
            await _tokenCacheManager.RetryWithToken(this, async (accessToken, _) =>
            {
                var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
                {
                    Path = $"api/user/device/status"
                };

                var body = JsonSerializer.Serialize(new
                {
                    deviceid = deviceId,
                    @params = parameters,
                    appid = EwelinkHelper.Constants.APP_ID,
                    nonce = EwelinkHelper.GenerateNonce(),
                    ts = EwelinkHelper.GenerateTimestamp(),
                    version = EwelinkHelper.Constants.VERSION
                });

                var request = new HttpRequestMessage(HttpMethod.Post, urlBuilder.ToString());
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
            });
        }

        async Task<Token> ITokenProvider.GetTokenAsync()
        {
            var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"api/user/login"
            };

            var body = JsonSerializer.Serialize(new
            {
                email = _config.Email,
                password = _config.Password,
                version = EwelinkHelper.Constants.VERSION,
                ts = EwelinkHelper.GenerateTimestamp(),
                nonce = EwelinkHelper.GenerateNonce(),
                os = EwelinkHelper.Constants.OS,
                appid = EwelinkHelper.Constants.APP_ID,
                imei = EwelinkHelper.GenerateFakeImei(),
                model = EwelinkHelper.Constants.MODEL,
                romVersion = EwelinkHelper.Constants.ROM_VERSION,
                appVersion = EwelinkHelper.Constants.APP_VERSION,
            });

            var signature = _cryptoService.HmacSha256Base64(body, EwelinkHelper.Constants.APP_SECRET);

            var request = new HttpRequestMessage(HttpMethod.Post, urlBuilder.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Sign", signature);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EwelinkDtoLoginResult>(responseContent);
            return new Token(result.at, result.rt, ("apiKey", result.apikey));
        }

        Task<Token> ITokenProvider.RefreshTokenAsync(string refreshToken)
        {
            return Task.FromResult<Token>(null);
        }
    }

    public class EwelinkDtoLoginResult
    {
        public string at { get; set; }
        public string rt { get; set; }
        public string apikey { get; set; }
    }

    public class EwelinkDtoDeviceList
    {
        public int error { get; set; }
        public EwelinkDtoDevice[] devicelist { get; set; }
    }

    public class EwelinkDtoDevice
    {
        public string apikey { get; set; }
        public string deviceid { get; set; }
        public int uiid { get; set; }
        public bool online { get; set; }
        public JsonElement @params { get; set; }
    }
}
