using Distvisor.App.Common;
using Distvisor.App.Core.Aggregates;
using Distvisor.App.HomeBox.Aggregates;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task SyncDevicesAsync()
        {
            var gatewayDevices = await _gatewayClient.GetDevicesAsync();
            foreach (var gatewayDevice in gatewayDevices.Devices)
            {
                var deviceEntity = await _appDbContext.HomeboxDevices.SingleOrDefaultAsync(x => x.GatewayDeviceId == gatewayDevice.DeviceId);
                var deviceAggregate = deviceEntity?.Id != null
                    ? _aggregateContext.Get<Device>(deviceEntity.Id)
                    : CreateDeviceAggregate();

                deviceAggregate.SyncWithGateway(gatewayDevice);
            }

            _aggregateContext.Commit();
        }

        private Device CreateDeviceAggregate()
        {
            var aggregate = new Device(Guid.NewGuid());
            _aggregateContext.Add(aggregate);
            return aggregate;
        }
    }
}
