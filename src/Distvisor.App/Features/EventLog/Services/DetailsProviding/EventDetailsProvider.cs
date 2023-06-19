using Distvisor.App.Core.Events;
using Distvisor.App.EventLog.Services.DetailsProviding;
using System;
using System.Text.Json;

namespace Distvisor.App.Features.EventLog.Services.DetailsProviding
{
    public class EventDetailsProvider : DetailsProvider<IEvent, EventDetails>, IEventDetailsProvider
    {
        public EventDetailsProvider(ISensitiveDataMaskConfiguration configuration)
            : base(configuration) { }

        protected override Func<IEvent, EventDetails> GetDetailsFactory(IEvent firstEvent)
        {
            var eventType = firstEvent.GetType();
            var eventTypeDisplayName = GetDisplayName(eventType);
            var aggregateTypeDisplayName = GetDisplayName(firstEvent.AggregateType);
            var maskSensitiveDataSerializerOptions = GetMaskSensitiveDataSerializerOptions();
            return (@event) => new EventDetails
            {
                EventTypeDisplayName = eventTypeDisplayName,
                AggregateTypeDisplayName = aggregateTypeDisplayName,
                MaskedPayload = JsonSerializer.SerializeToDocument(@event, eventType, maskSensitiveDataSerializerOptions)
            };
        }
    }
}
