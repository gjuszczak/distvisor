using Distvisor.App.Core.Entities;
using Distvisor.App.Features.HomeBox.Enums;
using System;
using System.Text.Json;

namespace Distvisor.App.Features.HomeBox.Entities
{
    public class DeviceEntity : AuditableEntity
    {
        public Guid Id { get; set; }
        public string GatewayDeviceId { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public bool IsOnline { get; set; }
        public JsonElement Params { get; set; }
        public DeviceType Type { get; set; }
        public string Location { get; set; }
    }
}
