using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Events;
using Distvisor.App.HomeBox.Enums;
using Distvisor.App.HomeBox.ValueObjects;
using System;
using System.Text.Json;

namespace Distvisor.App.HomeBox.Aggregates
{
    public class Device : AggregateRoot
    {
        public string GatewayId { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public bool Online { get; set; }
        public JsonElement Params { get; set; }
        public DeviceType Type { get; set; }
        public string Location { get; set; }

        public Device() { }

        public Device(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public void SyncWithGateway(GatewayDeviceDetails gatewayDevice)
        {
            throw new NotImplementedException();
        }

        protected override void Apply(IEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
