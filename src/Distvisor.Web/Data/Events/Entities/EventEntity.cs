using System;
using System.Text.Json;

namespace Distvisor.Web.Data.Events.Entities
{
    public class EventEntity
    {
        public EventEntity()
        {
        }

        public EventEntity(object payload)
        {
            var type = payload.GetType();
            var json = JsonSerializer.Serialize(payload, type);
            PublishDateUtc = DateTime.UtcNow;
            PayloadType = type.ToString();
            PayloadValue = JsonDocument.Parse(json);
        }

        public int Id { get; set; }
        public DateTime PublishDateUtc { get; set; }
        public string PayloadType { get; set; }
        public JsonDocument PayloadValue { get; set; }

        public object ToPayload()
        {
            var type = Type.GetType(PayloadType);
            var payload = JsonSerializer.Deserialize(PayloadValue.ToString(), type);
            return payload;
        }
    }
}
