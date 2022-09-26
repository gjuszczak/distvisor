using Distvisor.App.Core.Aggregates;
using Distvisor.App.EventLog.Services.DetailsProviding;
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

        public static AggregateDto FromAggregate(IAggregateRoot aggregate, AggregateDetails details)
        {
            return new AggregateDto
            {
                AggregateId = aggregate.AggregateId,
                Version = aggregate.Version,
                AggregateType = details.AggregateType,
                AggregateTypeDisplayName = details.AggregateTypeDisplayName,
                MaskedPayload = details.MaskedPayload
            };
        }
    }
}
