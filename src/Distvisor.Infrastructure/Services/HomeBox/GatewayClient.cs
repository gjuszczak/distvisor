using Distvisor.App.Features.HomeBox.Enums;
using Distvisor.App.Features.HomeBox.Services.Gateway;
using Distvisor.App.Features.HomeBox.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Services.HomeBox
{
    public class GatewayClient : IGatewayClient
    {
        private readonly HttpClient _httpClient;
        private readonly IGatewayAuthenticationPolicy _authPolicy;

        public GatewayClient(HttpClient client, IGatewayAuthenticationPolicy authPolicy)
        {
            _httpClient = client;
            _authPolicy = authPolicy;
        }

        public async Task<GetDevicesResponse> GetDevicesAsync(CancellationToken token = default)
        {
            return await _authPolicy.ExecuteWithTokenAsync(async (accessToken) =>
            {
                var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
                {
                    Path = $"v2/device/thing"
                };

                var url = urlBuilder.ToString();

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.SendAsync(request, token);
                response.EnsureSuccessStatusCode();

                using var streamContent =  await response.Content.ReadAsStreamAsync(token);
                using var jsonContent = await JsonSerializer.DeserializeAsync<JsonDocument>(streamContent);

                if (IsErrorResponse(jsonContent, out var error, out var message))
                {
                    throw new Exception($"GetDevicesAsync -> gateway returned an error = {error}. {message}");
                }

                var devices = ParseDeviceDetails(jsonContent).ToArray();

                return new GetDevicesResponse
                {
                    Devices = devices
                };
            });
        }

        public Task SetDeviceParamsAsync(string deviceId, object parameters, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        private static bool IsErrorResponse(JsonDocument jsonContent, out int error, out string message)
        {
            error = jsonContent.RootElement.GetProperty("error").GetInt32();
            message = jsonContent.RootElement.GetProperty("msg").GetString();
            return error != 0;
        }

        private static IEnumerable<GatewayDeviceDetails> ParseDeviceDetails(JsonDocument jsonContent)
        {
            var thingList = jsonContent.RootElement.GetProperty("data").GetProperty("thingList").EnumerateArray();
            foreach(var device in thingList)
            {
                if (device.GetProperty("itemType").GetInt32() != 1)
                    continue;

                var deviceData = device.GetProperty("itemData");
                var name = deviceData.GetProperty("name").GetString();
                var deviceId = deviceData.GetProperty("deviceid").GetString();
                var uiid = deviceData.GetProperty("extra").GetProperty("uiid").GetInt32();
                var online = deviceData.GetProperty("online").GetBoolean();
                var @params = deviceData.GetProperty("params").Clone();
                var deviceType = UiidToDeviceType(uiid);

                yield return new GatewayDeviceDetails(name, deviceId, deviceType, online, @params);
            }
        }

        private static DeviceType UiidToDeviceType(int gatewayDeviceType)
        {
            return gatewayDeviceType switch
            {
                1 => DeviceType.Switch,
                28 => DeviceType.RfBridge,
                59 => DeviceType.RgbLight,
                104 => DeviceType.RgbwLight,
                _ => DeviceType.Unknown,
            };
        }
    }
}
