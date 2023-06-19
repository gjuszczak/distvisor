using Distvisor.App.Core.ValueObjects;
using Distvisor.App.Features.HomeBox.Enums;
using System.Collections.Generic;
using System.Text.Json;

namespace Distvisor.App.Features.HomeBox.ValueObjects
{
    public class GatewayDeviceDetails : ValueObject
    {
        public GatewayDeviceDetails(string name, string deviceId, DeviceType type, bool isOnline, JsonElement @params)
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
        public JsonElement Params { get; init; }

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
