using Distvisor.App.Core.Events;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Distvisor.App.EventLog.Services.EventDetails
{
    public class EventDetailsProvider : IEventDetailsProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<(Type eventType, Type aggregateType), Func<JsonDocument, EventDetails>> _eventDetailsProviders;

        public EventDetailsProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _eventDetailsProviders = new ConcurrentDictionary<(Type eventType, Type aggregateType), Func<JsonDocument, EventDetails>>();
        }

        public EventDetails GetEventDetails(EventEntity eventEntity)
        {
            var eventType = Type.GetType(eventEntity.EventType);
            var aggregateType = Type.GetType(eventEntity.AggregateType);
            var providerKey = (eventType, aggregateType);
            var provider = _eventDetailsProviders.GetOrAdd(providerKey,
                key => CreateProvider(key.eventType, key.aggregateType, _serviceProvider));
            return provider(eventEntity.Data);
        }

        private static Func<JsonDocument, EventDetails> CreateProvider(
            Type eventType,
            Type aggregateType,
            IServiceProvider serviceProvider)
        {
            var eventTypeDisplayName = GenerateDisplayName(eventType);
            var aggregateTypeDisplayName = GenerateDisplayName(aggregateType);
            var maskingServiceType = typeof(IEventMaskingService<>).MakeGenericType(eventType);
            var maskingService = (IEventMaskingService)serviceProvider.GetService(maskingServiceType);
            return CreateProvider(eventTypeDisplayName, aggregateTypeDisplayName, maskingService);
        }

        private static Func<JsonDocument, EventDetails> CreateProvider(
            string eventTypeDisplayName,
            string aggregateTypeDisplayName,
            IEventMaskingService maskingService)
        {
            return (eventPayload) => new EventDetails
            {
                EventTypeDisplayName = eventTypeDisplayName,
                AggregateTypeDisplayName = aggregateTypeDisplayName,
                MaskedPayload = maskingService?.Mask(eventPayload) ?? eventPayload
            };
        }

        private static string GenerateDisplayName(Type type)
        {
            return Regex.Replace(type.Name, "([a-z0-9])([A-Z])|([A-Z])([A-Z][a-z])", "$1$3 $2$4");
        }
    }
}
