using System.Text.Json;

namespace Distvisor.App.HomeBox.Services.Gateway
{
    public class GatewayDeviceDetails
    {
        public string Name { get; set; }
        public string Deviceid { get; set; }
        public int DeviceType { get; set; }
        public bool IsOnline { get; set; }
        public JsonElement Params { get; set; }
    }
}
