using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.HomeBox.Services.Gateway
{
    public interface IGatewayClient
    {
        Task<GetDevicesResponse> GetDevicesAsync(CancellationToken cancellationToken = default);
        Task SetDeviceParamsAsync(string deviceId, object parameters, CancellationToken cancellationToken = default);
    }
}
