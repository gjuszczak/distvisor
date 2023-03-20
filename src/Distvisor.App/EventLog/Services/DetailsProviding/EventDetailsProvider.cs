using Distvisor.App.Core.Events;
using System;
using System.Text.Json;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public class EventDetailsProvider : DetailsProvider<(Type eventType, Type aggregateType), EventEntity, EventDetails>, IEventDetailsProvider
    {
        private readonly IEventEntityBuilder _eventEntityBuilder;

        public EventDetailsProvider(IEventEntityBuilder eventEntityBuilder)
        {
            _eventEntityBuilder = eventEntityBuilder;
        }

        protected override (Type eventType, Type aggregateType) GetDetailsFactoryKey(EventEntity @event)
        {
            return (Type.GetType(@event.EventType), Type.GetType(@event.AggregateType));
        }

        protected override Func<EventEntity, EventDetails> GetDetailsFactory((Type eventType, Type aggregateType) factoryKey)
        {
            var (eventType, aggregateType) = factoryKey;
            var eventTypeDisplayName = GetDisplayName(eventType);
            var aggregateTypeDisplayName = GetDisplayName(aggregateType);
            var maskSensitiveDataSerializerOptions = GetMaskSensitiveDataSerializerOptions();
            return (@event) => new EventDetails
            {
                EventTypeDisplayName = eventTypeDisplayName,
                AggregateTypeDisplayName = aggregateTypeDisplayName,
                MaskedPayload = JsonSerializer.SerializeToDocument(_eventEntityBuilder.FromEventEntity(@event), eventType, maskSensitiveDataSerializerOptions)
            };
        }
    }
}
