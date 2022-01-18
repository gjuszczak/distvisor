using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Services.Gateway
{
    public interface IGatewayClient
    {
        Task<GetDevicesResponse> GetDevicesAsync();
        Task SetDeviceParamsAsync(string deviceId, object parameters);
    }
}
