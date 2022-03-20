using Distvisor.App.Core.ValueObjects;
using System.Collections.Generic;
using System.Text.Json;

namespace Distvisor.App.HomeBox.ValueObjects
{
    public class GatewayDeviceDetails : ValueObject
    {
        public GatewayDeviceDetails(string name, string deviceId, int deviceType, bool isOnline, JsonElement @params)
        {
            Name = name;
            DeviceId = deviceId;
            DeviceType = deviceType;
            IsOnline = isOnline;
            Params = @params;
        }

        public string Name { get; init; }
        public string DeviceId { get; init; }
        public int DeviceType { get; init; }
        public bool IsOnline { get; init; }
        public JsonElement Params { get; init; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return DeviceId;
            yield return DeviceType;
            yield return IsOnline;
            yield return Params.ToString();
        }
    }
}
