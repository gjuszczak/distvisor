using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Events;
using Distvisor.App.HomeBox.Enums;
using Distvisor.App.HomeBox.Events;
using Distvisor.App.HomeBox.ValueObjects;
using System;
using System.Text.Json;

namespace Distvisor.App.HomeBox.Aggregates
{
    public class Device : AggregateRoot
    {
        public string GatewayDeviceId { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public bool IsOnline { get; set; }
        public JsonDocument Params { get; set; }
        public DeviceType Type { get; set; }
        public string Location { get; set; }

        public Device() { }

        public Device(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public void SyncWithGateway(GatewayDeviceDetails gatewayDeviceDetails)
        {
            ApplyChange(new DeviceSyncedWithGateway(gatewayDeviceDetails));
        }

        protected override void Apply(IEvent @event)
        {
            switch (@event)
            {
                case DeviceSyncedWithGateway deviceSyncedWithGateway:
                    Apply(deviceSyncedWithGateway);
                    break;
            }
        }

        private void Apply(DeviceSyncedWithGateway deviceSyncedWithGateway)
        {
            Name = deviceSyncedWithGateway.DeviceDetails.Name;
            GatewayDeviceId = deviceSyncedWithGateway.DeviceDetails.DeviceId;
            Type = deviceSyncedWithGateway.DeviceDetails.Type;
            IsOnline = deviceSyncedWithGateway.DeviceDetails.IsOnline;
            Params = deviceSyncedWithGateway.DeviceDetails.Params;
        }
    }
}
