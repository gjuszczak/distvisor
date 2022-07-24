using Distvisor.App.Core.Events;
using System;

namespace Distvisor.App.EventsLog.Qureies.GetEvents
{
    public class EventsLogEntryDto
    {
        public Guid EventId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string EventType { get; set; }
        public string EventTypeDisplayName { get; set; }
        public object MaskedPayload { get; set; }
        public Guid AggregateId { get; set; }
        public string AggregateType { get; set; }
        public string AggregateTypeDisplayName { get; set; }
        public int Version { get; set; }
        public Guid CorrelationId { get; set; }

        public static EventsLogEntryDto FromEntity(EventEntity entity, string eventTypeDisplayName, string aggregateTypeDisplayName, object maskedPayload)
        {
            return new EventsLogEntryDto
            {
                EventId = entity.EventId,
                TimeStamp = entity.TimeStamp,
                EventType = entity.EventType,
                EventTypeDisplayName = eventTypeDisplayName,
                MaskedPayload = maskedPayload,
                AggregateId = entity.AggregateId,
                AggregateType = entity.AggregateType,
                AggregateTypeDisplayName = aggregateTypeDisplayName,
                Version = entity.Version,
                CorrelationId = entity.CorrelationId
            };
        }
    }
}
