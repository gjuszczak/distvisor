using Distvisor.App.HomeBox.Enums;
using Distvisor.App.HomeBox.Services.Gateway;
using Distvisor.App.HomeBox.ValueObjects;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Services.HomeBox
{
    public class GatewayClient : IGatewayClient
    {
        private readonly HttpClient _httpClient;
        private readonly GatewayConfiguration _config;
        private readonly IGatewayAuthenticationPolicy _authPolicy;

        public GatewayClient(HttpClient client, IOptions<GatewayConfiguration> config, IGatewayAuthenticationPolicy authPolicy)
        {
            _httpClient = client;
            _config = config.Value;
            _authPolicy = authPolicy;
        }

        public async Task<GetDevicesResponse> GetDevicesAsync()
        {
            return await _authPolicy.ExecuteWithTokenAsync(async (accessToken) =>
            {
                var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
                {
                    Path = $"api/user/device"
                };

                var queryParams = new Dictionary<string, string>
                {
                    ["lang"] = GatewayHelper.Constants.LANG_EN,
                    ["appid"] = _config.AppId,
                    ["nonce"] = GatewayHelper.GenerateNonce(),
                    ["ts"] = GatewayHelper.GenerateTimestamp(),
                    ["version"] = GatewayHelper.Constants.VERSION,
                    ["getTags"] = GatewayHelper.Constants.GET_TAGS_OFF,
                };

                var url = QueryHelpers.AddQueryString(urlBuilder.ToString(), queryParams);

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GatewayDeviceListDto>(responseContent);
                return new GetDevicesResponse
                {
                    Devices = result.devicelist.Select(d => new GatewayDeviceDetails
                    (
                        d.name,
                        d.deviceid,
                        DeviceType.FromGatewayDeviceType(d.uiid),
                        d.online,
                        d.@params
                    )).ToArray()
                };
            });
        }

        public Task SetDeviceParamsAsync(string deviceId, object parameters)
        {
            throw new System.NotImplementedException();
        }
    }

    public class GatewayDeviceListDto
    {
        public int error { get; set; }
        public GatewayDeviceDto[] devicelist { get; set; }
    }

    public class GatewayDeviceDto
    {
        public string apikey { get; set; }
        public string name { get; set; }
        public string deviceid { get; set; }
        public int uiid { get; set; }
        public bool online { get; set; }
        public JsonDocument @params { get; set; }
    }
}
