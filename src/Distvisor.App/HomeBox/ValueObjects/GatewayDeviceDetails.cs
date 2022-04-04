using Distvisor.App.Core.ValueObjects;
using Distvisor.App.HomeBox.Enums;
using System.Collections.Generic;
using System.Text.Json;

namespace Distvisor.App.HomeBox.ValueObjects
{
    public class GatewayDeviceDetails : ValueObject
    {
        public GatewayDeviceDetails(string name, string deviceId, DeviceType type, bool isOnline, JsonDocument @params)
        {
            Name = name;
            DeviceId = deviceId;
            Type = type;
            IsOnline = isOnline;
            Params = @params;
        }

        public string Name { get; init; }
        public string DeviceId { get; init; }
        public DeviceType Type { get; init; }
        public bool IsOnline { get; init; }
        public JsonDocument Params { get; init; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return DeviceId;
            yield return Type;
            yield return IsOnline;
            yield return Params.ToString();
        }
    }
}
