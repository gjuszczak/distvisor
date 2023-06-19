using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.HomeBox.Services.Gateway
{
    public interface IGatewayDevicesSyncService
    {
        Task SyncDevicesAsync(CancellationToken cancellationToken = default);
    }
}
