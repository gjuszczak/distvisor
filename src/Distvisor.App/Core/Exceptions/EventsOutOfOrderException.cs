using System;

namespace Distvisor.App.Core.Exceptions
{
    public class EventsOutOfOrderException : Exception
    {
        public EventsOutOfOrderException(Guid aggregateId, Type aggregateType, int currentVersion, int providedEventVersion)
            : base($"Events are out of order for aggregate {aggregateType.FullName}[id:{aggregateId}, v:{currentVersion}]. Event version: {providedEventVersion}.")
        {
            AggregateId = aggregateId;
            AggregateType = aggregateType;
            CurrentVersion = currentVersion;
            ProvidedEventVersion = providedEventVersion;
        }

        public Guid AggregateId { get; set; }
        public Type AggregateType { get; set; }
        public int CurrentVersion { get; set; }
        public int ProvidedEventVersion { get; set; }
    }
}
