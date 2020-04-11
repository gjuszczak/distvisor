using LiteDB;
using System;

namespace Distvisor.Web.Data.EventSourcing.Core
{
    public class EventEntity
    {
        public EventEntity()
        {
        }

        public EventEntity(object payload)
        {
            var type = payload.GetType();
            PublishDateUtc = DateTime.UtcNow;
            PayloadType = type.ToString();
            PayloadValue = BsonMapper.Global.Serialize(type, payload);
        }

        public int Id { get; set; }
        public DateTime PublishDateUtc { get; set; }
        public string PayloadType { get; set; }
        public BsonValue PayloadValue { get; set; }

        public object ToPayload()
        {
            var type = Type.GetType(PayloadType);
            var payload = BsonMapper.Global.Deserialize(type, PayloadValue);
            return payload;
        }
    }
}
