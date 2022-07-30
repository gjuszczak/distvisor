using Distvisor.App.Core.Events;
using System;
using System.Text.Json;

namespace Distvisor.App.EventLog.Services.EventDetails
{
    public class EventDetailsProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public EventDetailsProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public EventDetails GetEventDetails(EventEntity eventEntity)
        {
            var eventType = Type.GetType(eventEntity.EventType);
            var aggregateType = Type.GetType(eventEntity.AggregateType);
            var maskingServiceType = typeof(IEventMaskingService<>).MakeGenericType(eventType);
            var maskingService = (IEventMaskingService)_serviceProvider.GetService(maskingServiceType);
            var provider = CreateProvider(eventType.Name, aggregateType.Name, maskingService);
            return provider(eventEntity.Data);
        }

        private Func<JsonDocument, EventDetails> CreateProvider(
            string eventTypeDisplayName, 
            string aggregateTypeDisplayName,
            IEventMaskingService maskingService)
        {
            return (payload) => new EventDetails
            {
                EventTypeDisplayName = eventTypeDisplayName,
                AggregateTypeDisplayName = aggregateTypeDisplayName,
                MaskedPayload = maskingService?.Mask(payload) ?? payload
            };
        }
    }
}
