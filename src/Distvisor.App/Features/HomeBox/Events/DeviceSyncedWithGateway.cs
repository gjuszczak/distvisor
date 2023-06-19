using Distvisor.App.Core.Events;
using Distvisor.App.Features.HomeBox.ValueObjects;
using Distvisor.App.Features.HomeBox.Entities;
using System.Threading;
using System.Threading.Tasks;
using Distvisor.App.Features.Common.Interfaces;
using System.Xml.Linq;
using System;

namespace Distvisor.App.Features.HomeBox.Events
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
            var entity = _appDbContext.HomeboxDevices.Find(@event.AggregateId)
                ?? _appDbContext.HomeboxDevices.Add(new DeviceEntity()).Entity;

            entity.Id = @event.AggregateId;
            entity.Name = @event.DeviceDetails.Name;
            entity.GatewayDeviceId = @event.DeviceDetails.DeviceId;
            entity.Type = @event.DeviceDetails.Type;
            entity.IsOnline = @event.DeviceDetails.IsOnline;
            entity.Params = @event.DeviceDetails.Params;

            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
