using System;

namespace Distvisor.App.EventLog.Qureies.GetAggregate
{
    public class AggregateDto
    {
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public object MaskedPayload { get; set; }
        public string AggregateType { get; set; }
        public string AggregateTypeDisplayName { get; set; }
    }
}
