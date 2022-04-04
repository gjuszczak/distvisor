using Distvisor.App.Core.Enums;

namespace Distvisor.App.HomeBox.Enums
{
    public class DeviceType: Enumeration
    {
        public static DeviceType Unknown = new(1, nameof(Unknown));
        public static DeviceType RgbLight = new(2, nameof(RgbLight));
        public static DeviceType RgbwLight = new(3, nameof(RgbwLight));
        public static DeviceType Switch = new(4, nameof(Switch));

        private DeviceType(int id, string name) : base(id, name) { }

        public static DeviceType FromGatewayDeviceType(int gatewayDeviceType)        
        {
            return gatewayDeviceType switch
            {
                1 => Switch,
                59 => RgbLight,
                104 => RgbwLight,
                _ => Unknown,
            };
        }
    }
}
