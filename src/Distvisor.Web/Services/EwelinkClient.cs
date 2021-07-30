using Distvisor.Web.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IEwelinkClient : IAuthTokenProvider
    {
        Task LoginAsync(string email, string password);
        Task<EwelinkDtoDeviceList> GetDevicesAsync();
        Task SetDeviceParamsAsync(string deviceId, object parameters);
    }

    public class EwelinkClient : IEwelinkClient
    {
        private readonly HttpClient _httpClient;
        private readonly EwelinkConfiguration _config;
        private readonly ICryptoService _cryptoService;
        private readonly IAuthTokenCacheManager _tokenCacheManager;

        public EwelinkClient(HttpClient client,
            IOptions<EwelinkConfiguration> config,
            ICryptoService cryptoService,
            IAuthTokenCacheFactory tokenCacheFactory)
        {
            _httpClient = client;
            _config = config.Value;
            _cryptoService = cryptoService;
            _tokenCacheManager = tokenCacheFactory.Create(this, typeof(EwelinkClient).FullName);
        }

        public async Task LoginAsync(string email, string password)
        {
            var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"api/user/login"
            };

            var body = JsonSerializer.Serialize(new
            {
                email,
                password,
                appid = _config.AppId,
                nonce = EwelinkHelper.GenerateNonce(),
                ts = EwelinkHelper.GenerateTimestamp(),
                version = EwelinkHelper.Constants.VERSION,
            });

            var url = urlBuilder.ToString();

            var signature = _cryptoService.HmacSha256Base64(body, _config.AppSecret);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Sign", signature);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EwelinkDtoLoginResult>(responseContent);
            var token = new AuthToken(result.at, result.rt, ("apiKey", result.apikey));
            _tokenCacheManager.SetAuthToken(token);
        }

        public async Task<EwelinkDtoDeviceList> GetDevicesAsync()
        {
            return await _tokenCacheManager.RetryWithTokenAsync(async (accessToken, _) =>
            {
                var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
                {
                    Path = $"api/user/device"
                };

                var queryParams = new Dictionary<string, string>
                {
                    ["lang"] = EwelinkHelper.Constants.LANG_EN,
                    ["appid"] = _config.AppId,
                    ["nonce"] = EwelinkHelper.GenerateNonce(),
                    ["ts"] = EwelinkHelper.GenerateTimestamp(),
                    ["version"] = EwelinkHelper.Constants.VERSION,
                    ["getTags"] = EwelinkHelper.Constants.GET_TAGS_OFF,
                };

                var url = QueryHelpers.AddQueryString(urlBuilder.ToString(), queryParams);

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<EwelinkDtoDeviceList>(responseContent);
                return result;
            });
        }

        public async Task SetDeviceParamsAsync(string deviceId, object parameters)
        {
            await _tokenCacheManager.RetryWithTokenAsync(async (accessToken, _) =>
            {
                var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
                {
                    Path = $"api/user/device/status"
                };

                var body = JsonSerializer.Serialize(new
                {
                    deviceid = deviceId,
                    @params = parameters,
                    appid = _config.AppId,
                    nonce = EwelinkHelper.GenerateNonce(),
                    ts = EwelinkHelper.GenerateTimestamp(),
                    version = EwelinkHelper.Constants.VERSION
                });

                var url = urlBuilder.ToString();

                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
            });
        }

        async Task<AuthToken> IAuthTokenProvider.RefreshAuthTokenAsync(string refreshToken)
        {
            var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"api/user/refresh"
            };

            var queryParams = new Dictionary<string, string>
            {
                ["rt"] = refreshToken,
                ["grantType"] = EwelinkHelper.Constants.GRANT_TYPE_REFRESH,
                ["appid"] = _config.AppId,
                ["nonce"] = EwelinkHelper.GenerateNonce(),
                ["ts"] = EwelinkHelper.GenerateTimestamp(),
                ["version"] = EwelinkHelper.Constants.VERSION,
            };

            var url = QueryHelpers.AddQueryString(urlBuilder.ToString(), queryParams);

            var signatureBase = string.Join('&', queryParams.OrderBy(x => x.Key).Select(x => $"{x.Key}={x.Value}"));
            var signature = _cryptoService.HmacSha256Base64(signatureBase, _config.AppSecret);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Sign", signature);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EwelinkDtoLoginResult>(responseContent);
            return new AuthToken(result.at, result.rt, ("apiKey", result.apikey));
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
        public string name { get; set; }
        public string deviceid { get; set; }
        public int uiid { get; set; }
        public bool online { get; set; }
        public JsonElement @params { get; set; }
    }
}
