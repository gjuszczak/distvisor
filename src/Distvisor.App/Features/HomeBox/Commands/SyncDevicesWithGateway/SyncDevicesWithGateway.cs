using Distvisor.App.Core.Commands;
using Distvisor.App.Features.HomeBox.Services.Gateway;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.HomeBox.Commands.SyncDevicesWithGateway
{
    public class SyncDevicesWithGateway : Command
    {
    }

    public class SyncDevicesWithGatewayHandler : ICommandHandler<SyncDevicesWithGateway>
    {
        private readonly IGatewayDevicesSyncService _devicesSyncService;

        public SyncDevicesWithGatewayHandler(IGatewayDevicesSyncService deviceSyncService)
        {
            _devicesSyncService = deviceSyncService;
        }

        public async Task<Guid> Handle(SyncDevicesWithGateway request, CancellationToken cancellationToken)
        {
            await _devicesSyncService.SyncDevicesAsync(cancellationToken);
            return request.Id;
        }
    }
}
