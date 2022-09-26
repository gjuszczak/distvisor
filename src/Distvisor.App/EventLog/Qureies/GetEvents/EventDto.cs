using Distvisor.App.Core.Events;
using Distvisor.App.EventLog.Services.DetailsProviding;
using System;

namespace Distvisor.App.EventLog.Qureies.GetEvents
{
    public class EventDto
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

        public static EventDto FromEntity(EventEntity entity, EventDetails details)
        {
            return new EventDto
            {
                EventId = entity.EventId,
                TimeStamp = entity.TimeStamp,
                EventType = entity.EventType,
                EventTypeDisplayName = details?.EventTypeDisplayName,
                MaskedPayload = details?.MaskedPayload,
                AggregateId = entity.AggregateId,
                AggregateType = entity.AggregateType,
                AggregateTypeDisplayName = details?.AggregateTypeDisplayName,
                Version = entity.Version,
                CorrelationId = entity.CorrelationId
            };
        }
    }
}
