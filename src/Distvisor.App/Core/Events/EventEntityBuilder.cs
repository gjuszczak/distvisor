using Distvisor.App.Core.Serialization;
using System;
using System.Text.Json;

namespace Distvisor.App.Core.Events
{
    public class EventEntityBuilder : IEventEntityBuilder
    {
        public virtual EventEntity ToEventEntity(IEvent @event)
        {
            return new EventEntity
            {
                EventId = @event.EventId,
                EventType = @event.GetType().FullName,
                AggregateId = @event.AggregateId,
                AggregateType = @event.AggregateType.FullName,
                Version = @event.Version,
                CorrelationId = @event.CorrelationId,
                TimeStamp = @event.TimeStamp,
                Data = SerializeEventData(@event)
            };
        }

        public virtual IEvent FromEventEntity(EventEntity eventEntity)
        {
            var @event = (IEvent)eventEntity.Data.Deserialize(Type.GetType(eventEntity.EventType), JsonDefaults.SerializerOptions);
            @event.EventId = eventEntity.EventId;
            @event.AggregateId = eventEntity.AggregateId;
            @event.AggregateType = Type.GetType(eventEntity.AggregateType);
            @event.Version = eventEntity.Version;
            @event.CorrelationId = eventEntity.CorrelationId;
            @event.TimeStamp = eventEntity.TimeStamp;
            return @event;
        }

        protected virtual JsonElement SerializeEventData(IEvent @event)
        {
            var eventData = JsonSerializer.SerializeToElement(@event, @event.GetType(), JsonDefaults.SerializerOptions);
            return eventData;
        }
    }
}
