using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Services.Gateway
{
    public interface IGatewayDevicesSyncService
    {
        Task SyncDevicesAsync();
    }
}
