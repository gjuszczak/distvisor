using Distvisor.App.Core.Aggregates;
using Distvisor.App.Features.HomeBox.Enums;
using Distvisor.App.Features.HomeBox.Events;
using Distvisor.App.Features.HomeBox.ValueObjects;
using System;
using System.Text.Json;

namespace Distvisor.App.Features.HomeBox.Aggregates
{
    public class Device : AggregateRoot
    {
        public string GatewayDeviceId { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public bool IsOnline { get; set; }
        public JsonElement Params { get; set; }
        public DeviceType Type { get; set; }
        public string Location { get; set; }

        public Device()
        {
            RegisterEventHandler<DeviceSyncedWithGateway>(Apply);
        }

        public Device(Guid aggregateId) : this()
        {
            AggregateId = aggregateId;
        }

        public void SyncWithGateway(GatewayDeviceDetails gatewayDeviceDetails)
        {
            ApplyEvent(new DeviceSyncedWithGateway(gatewayDeviceDetails));
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
