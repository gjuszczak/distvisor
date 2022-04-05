using Distvisor.App.HomeBox.Entities;
using Distvisor.App.HomeBox.Enums;
using System;
using System.Text.Json;

namespace Distvisor.App.HomeBox.Queries.GetDevices
{
    public class DeviceDto
    {
        public Guid Id { get; set; }
        public string GatewayDeviceId { get; set; }
        public string Name { get; set; }
        public string Header { get; set; }
        public bool IsOnline { get; set; }
        public JsonDocument Params { get; set; }
        public DeviceType Type { get; set; }
        public string Location { get; set; }

        public static DeviceDto FromEntity(DeviceEntity entity)
        {
            return new DeviceDto
            {
                Id = entity.Id,
                GatewayDeviceId = entity.GatewayDeviceId,
                Name = entity.Name,
                Header = entity.Header,
                IsOnline = entity.IsOnline,
                Params = entity.Params,
                Type = entity.Type,
                Location = entity.Location
            };
        }
    }
}
