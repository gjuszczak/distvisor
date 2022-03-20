using Distvisor.App.HomeBox.ValueObjects;
using System.Collections.Generic;

namespace Distvisor.App.HomeBox.Services.Gateway
{
    public class GetDevicesResponse
    {
        public IEnumerable<GatewayDeviceDetails> Devices { get; set; }
    }
}
