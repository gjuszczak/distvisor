using Distvisor.App.Features.HomeBox.ValueObjects;
using System.Collections.Generic;

namespace Distvisor.App.Features.HomeBox.Services.Gateway
{
    public class GetDevicesResponse
    {
        public IEnumerable<GatewayDeviceDetails> Devices { get; set; }
    }
}
