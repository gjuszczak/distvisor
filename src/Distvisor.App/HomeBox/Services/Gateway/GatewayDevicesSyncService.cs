using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Exceptions;
using Distvisor.App.HomeBox.Aggregates;
using Distvisor.App.HomeBox.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Services.Gateway
{
    public class GatewayDevicesSyncService : IGatewayDevicesSyncService
    {
        private readonly IAggregateContext _aggregateContext;
        private readonly IGatewayClient _gatewayClient;

        public GatewayDevicesSyncService(IAggregateContext aggregateContext, IGatewayClient gatewayClient)
        {
            _aggregateContext = aggregateContext;
            _gatewayClient = gatewayClient;
        }

        public async Task SyncDevicesAsync()
        {
            var gatewayDevices = await _gatewayClient.GetDevicesAsync();
            var deviceLinksList = GetOrCreateDeviceLinksListAggregate();
            var deviceLinksToSync = new List<DeviceLink>();

            foreach (var gatewayDevice in gatewayDevices.Devices)
            {
                var deviceAggregateId = deviceLinksList.DeviceLinks
                    .FirstOrDefault(link => link.DeviceGatewayId == gatewayDevice.DeviceId)?.DeviceAggregateId;

                var deviceAggregate = deviceAggregateId.HasValue
                    ? _aggregateContext.Get<Device>(deviceAggregateId.Value)
                    : CreateDeviceAggregate();

                deviceAggregate.SyncWithGateway(gatewayDevice);
                deviceLinksToSync.Add(new DeviceLink(deviceAggregate.AggregateId, gatewayDevice.DeviceId));
            }

            if (deviceLinksToSync.Any())
            {
                deviceLinksList.SyncWithGateway(deviceLinksToSync);
            }

            _aggregateContext.Commit();
        }

        private DeviceLinksList GetOrCreateDeviceLinksListAggregate()
        {
            try
            {
                return _aggregateContext.Get<DeviceLinksList>(AggregateConstIds.DeviceLinksList);
            }
            catch (AggregateNotFoundException)
            {
                var aggregate = new DeviceLinksList(AggregateConstIds.DeviceLinksList);
                _aggregateContext.Add(aggregate);
                return aggregate;
            }
        }

        private Device CreateDeviceAggregate()
        {
            var aggregate = new Device(Guid.NewGuid());
            _aggregateContext.Add(aggregate);
            return aggregate;
        }
    }
}
