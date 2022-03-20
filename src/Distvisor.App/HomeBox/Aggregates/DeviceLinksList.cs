using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Events;
using Distvisor.App.HomeBox.Events;
using Distvisor.App.HomeBox.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Distvisor.App.HomeBox.Aggregates
{
    public class DeviceLinksList : AggregateRoot
    {
        public IEnumerable<DeviceLink> DeviceLinks { get; private set; } = Enumerable.Empty<DeviceLink>();

        public DeviceLinksList() { }

        public DeviceLinksList(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public void SyncWithGateway(IEnumerable<DeviceLink> freshDeviceLinks)
        {
            var toAdd = freshDeviceLinks.Where(x => DeviceLinks.All(y => y.DeviceGatewayId != x.DeviceGatewayId)).ToArray();
            var toDelete = DeviceLinks.Where(x => freshDeviceLinks.All(y => y.DeviceGatewayId != x.DeviceGatewayId)).ToArray();
            if (toAdd.Any() || toDelete.Any())
            {
                ApplyChange(new DeviceLinksUpdated(toAdd, toDelete));
            }
        }

        protected override void Apply(IEvent @event)
        {
            switch (@event)
            {
                case DeviceLinksUpdated deviceLinksUpdated:
                    Apply(deviceLinksUpdated);
                    break;
            }
        }

        private void Apply(DeviceLinksUpdated @event)
        {
            DeviceLinks = DeviceLinks
                .Except(@event.Deleted)
                .Concat(@event.Added)
                .ToArray();
        }
    }
}
