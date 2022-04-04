using Distvisor.App.Common;
using Distvisor.App.Core.Events;
using Distvisor.App.HomeBox.Entities;
using Distvisor.App.HomeBox.Enums;
using Distvisor.App.HomeBox.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Events
{
    public class DeviceSyncedWithGateway : Event
    {
        public GatewayDeviceDetails DeviceDetails { get; init; }

        public DeviceSyncedWithGateway(GatewayDeviceDetails deviceDetails)
        {
            DeviceDetails = deviceDetails;
        }
    }

    public class DeviceSyncedWithGatewayHandler : IEventHandler<DeviceSyncedWithGateway>
    {
        private readonly IAppDbContext _appDbContext;
        public DeviceSyncedWithGatewayHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Handle(DeviceSyncedWithGateway @event, CancellationToken cancellationToken)
        {
            _appDbContext.HomeboxDevices.Add(new DeviceEntity
            {
                Id = @event.AggregateId,
                Name = @event.DeviceDetails.Name,
                GatewayDeviceId = @event.DeviceDetails.DeviceId,
                Type = @event.DeviceDetails.Type,
                IsOnline = @event.DeviceDetails.IsOnline,
                Params = @event.DeviceDetails.Params
            });
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
