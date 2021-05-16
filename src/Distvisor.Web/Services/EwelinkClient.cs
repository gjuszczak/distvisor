using Distvisor.Web.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IEwelinkClient
    {
        Task<string> GetAccessToken();
        Task<string> GetDevices(string accessToken);
    }

    public class EwelinkClient : IEwelinkClient
    {
        private readonly HttpClient _httpClient;
        private readonly EwelinkConfiguration _config;
        private readonly ICryptoService _cryptoService;

        public EwelinkClient(HttpClient client, IOptions<EwelinkConfiguration> config, ICryptoService cryptoService)
        {
            _httpClient = client;
            _config = config.Value;
            _cryptoService = cryptoService;
        }

        public async Task<string> GetAccessToken()
        {
            var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"api/user/login"
            };

            var body = JsonSerializer.Serialize(new
            {
                email = _config.Email,
                password = _config.Password,
                version = EwelinkConst.VERSION,
                ts = GenerateTimestamp(),
                nonce = GenerateNonce(),
                os = EwelinkConst.OS,
                appid = EwelinkConst.APP_ID,
                imei = GenerateFakeImei(),
                model = EwelinkConst.MODEL,
                romVersion = EwelinkConst.ROM_VERSION,
                appVersion = EwelinkConst.APP_VERSION,
            });

            var signature = _cryptoService.HmacSha256Base64(body, EwelinkConst.APP_SECRET);

            var request = new HttpRequestMessage(HttpMethod.Post, urlBuilder.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Sign", signature);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = DynamicDeserialize(responseContent, new { at = "" });
            return result.at;
        }

        public async Task<string> GetDevices(string accessToken)
        {
            var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"api/user/device"
            };

            var url = QueryHelpers.AddQueryString(urlBuilder.ToString(), new Dictionary<string, string>
            {
                ["lang"] = "en",
                ["getTags"] = "1",
                ["version"] = EwelinkConst.APP_VERSION,
                ["ts"] = GenerateTimestamp(),
                ["appid"] = EwelinkConst.APP_ID,
                ["imei"] = GenerateFakeImei(),
                ["os"] = EwelinkConst.OS,
                ["model"] = EwelinkConst.MODEL,
                ["romVersion"] = EwelinkConst.ROM_VERSION,
                ["appVersion"] = EwelinkConst.APP_VERSION,
            });

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private string GenerateNonce()
        {
            var nonce = new byte[15];
            _cryptoService.FillWithRandomNumbers(nonce);
            return Convert.ToBase64String(nonce);
        }

        private string GenerateTimestamp()
        {
            var seed = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var timestamp = Math.Floor(seed / 1000);
            return timestamp.ToString(CultureInfo.InvariantCulture);
        }

        private (string timestamp, string sequence) GenerateSequence()
        {
            var seed = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var timestamp = Math.Floor(seed / 1000);
            var sequence = Math.Floor(timestamp);
            return (timestamp.ToString(CultureInfo.InvariantCulture), sequence.ToString(CultureInfo.InvariantCulture));
        }

        private string GenerateFakeImei()
        {
            var random = new Random();
            var num1 = random.Next(1000, 9999);
            var num2 = random.Next(1000, 9999);

            return $"DF7425A0-{num1}-{num2}-9F5E-3BC9179E48FB";
        }

        private T DynamicDeserialize<T>(string json, T model)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        private static class EwelinkConst
        {
            public const string VERSION = "6";
            public const string OS = "android";
            public const string APP_ID = "oeVkj2lYFGnJu5XUtWisfW4utiN4u9Mq";
            public const string APP_SECRET = "6Nz4n0xA8s8qdxQf2GqurZj2Fs55FUvM";
            public const string MODEL = "";
            public const string ROM_VERSION = "";
            public const string APP_VERSION = "3.14.1";
            public const string APK_VERSION = "1.8";
        }
    }
}

