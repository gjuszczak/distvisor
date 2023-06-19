using Distvisor.App.Core.Events;
using Distvisor.App.Features.EventLog.Services.DetailsProviding;
using System;

namespace Distvisor.App.Features.EventLog.Qureies.GetEvents
{
    public class EventDto
    {
        public long EventId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string EventType { get; set; }
        public string EventTypeDisplayName { get; set; }
        public object MaskedPayload { get; set; }
        public Guid AggregateId { get; set; }
        public string AggregateType { get; set; }
        public string AggregateTypeDisplayName { get; set; }
        public int Version { get; set; }
        public Guid CorrelationId { get; set; }

        public static EventDto FromEvent(IEvent @event, EventDetails details)
        {
            return new EventDto
            {
                EventId = @event.EventId,
                TimeStamp = @event.TimeStamp,
                EventType = @event.GetType().FullName,
                EventTypeDisplayName = details?.EventTypeDisplayName,
                MaskedPayload = details?.MaskedPayload,
                AggregateId = @event.AggregateId,
                AggregateType = @event.AggregateType.FullName,
                AggregateTypeDisplayName = details?.AggregateTypeDisplayName,
                Version = @event.Version,
                CorrelationId = @event.CorrelationId
            };
        }
    }
}
