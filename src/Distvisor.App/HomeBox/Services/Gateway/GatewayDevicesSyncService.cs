using Distvisor.App.Common;
using Distvisor.App.Core.Aggregates;
using Distvisor.App.HomeBox.Aggregates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Services.Gateway
{
    public class GatewayDevicesSyncService : IGatewayDevicesSyncService
    {
        private readonly IAggregateContext _aggregateContext;
        private readonly IAppDbContext _appDbContext;
        private readonly IGatewayClient _gatewayClient;

        public GatewayDevicesSyncService(IAggregateContext aggregateContext, IAppDbContext appDbContext, IGatewayClient gatewayClient)
        {
            _aggregateContext = aggregateContext;
            _appDbContext = appDbContext;
            _gatewayClient = gatewayClient;
        }

        public async Task SyncDevicesAsync(CancellationToken cancellationToken = default)
        {
            var gatewayDevices = await _gatewayClient.GetDevicesAsync(cancellationToken);
            foreach (var gatewayDevice in gatewayDevices.Devices)
            {
                var deviceEntity = await _appDbContext.HomeboxDevices.SingleOrDefaultAsync(x => x.GatewayDeviceId == gatewayDevice.DeviceId, cancellationToken);
                var deviceAggregate = deviceEntity?.Id != null
                    ? await _aggregateContext.GetAsync<Device>(deviceEntity.Id, cancellationToken: cancellationToken)
                    : CreateDeviceAggregate();

                deviceAggregate.SyncWithGateway(gatewayDevice);
            }

            await _aggregateContext.CommitAsync(cancellationToken);
        }

        private Device CreateDeviceAggregate()
        {
            var aggregate = new Device(Guid.NewGuid());
            _aggregateContext.Add(aggregate);
            return aggregate;
        }
    }
}
