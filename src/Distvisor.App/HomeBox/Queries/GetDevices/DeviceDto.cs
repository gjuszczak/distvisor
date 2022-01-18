using System.Text.Json;

namespace Distvisor.App.HomeBox.Queries.GetDevices
{
    public class DeviceDto
    {
        public string Name { get; set; }
        public bool Online { get; set; }
        public JsonElement Params { get; set; }
    }
}
