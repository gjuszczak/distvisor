using System;
using System.IO;
using System.Text.Json;

namespace Distvisor.Web.Data.Events.Entities
{
    public class EventEntity
    {
        public EventEntity()
        {
        }

        public EventEntity(object payload, Type type)
        {
            var json = JsonSerializer.Serialize(payload, type);
            PublishDateUtc = DateTime.UtcNow;
            PayloadType = type.ToString();
            PayloadValue = JsonDocument.Parse(json);
            Success = true;
        }

        public int Id { get; set; }
        public DateTime PublishDateUtc { get; set; }
        public string PayloadType { get; set; }
        public bool Success { get; set; }
        public JsonDocument PayloadValue { get; set; }

        public object ToPayload()
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
            using var reader = new StreamReader(stream);
            PayloadValue.WriteTo(writer);
            writer.Flush();

            stream.Position = 0;
            var type = Type.GetType(PayloadType);
            var payload = JsonSerializer.Deserialize(reader.ReadToEnd(), type);
            return payload;
        }
    }
}
