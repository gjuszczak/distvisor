using Distvisor.App.Core.Events;
using System;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public class EventDetailsProvider : DetailsProvider<(Type eventType, Type aggregateType), EventEntity, EventDetails>, IEventDetailsProvider
    {
        public EventDetailsProvider(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override (Type eventType, Type aggregateType) GetDetailsFactoryKey(EventEntity @event)
        {
            return (Type.GetType(@event.EventType), Type.GetType(@event.AggregateType));
        }

        protected override Func<EventEntity, EventDetails> GetDetailsFactory((Type eventType, Type aggregateType) factoryKey)
        {
            var (eventType, aggregateType) = factoryKey;
            var eventTypeDisplayName = GetDisplayName(eventType);
            var aggregateTypeDisplayName = GetDisplayName(aggregateType);
            var maskingService = GetMaskingService(eventType);
            return (@event) => new EventDetails
            {
                EventTypeDisplayName = eventTypeDisplayName,
                AggregateTypeDisplayName = aggregateTypeDisplayName,
                MaskedPayload = maskingService?.Mask(@event.Data) ?? @event.Data
            };
        }
    }
}
