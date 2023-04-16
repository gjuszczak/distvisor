using Distvisor.App.Core.Aggregates;
using System;
using System.Text.Json;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public class AggregateDetailsProvider : DetailsProvider<IAggregateRoot, AggregateDetails>, IAggregateDetailsProvider
    {
        public AggregateDetailsProvider(ISensitiveDataMaskConfiguration configuration) 
            : base(configuration) { }

        protected override Func<IAggregateRoot, AggregateDetails> GetDetailsFactory(IAggregateRoot firstAggregate)
        {
            var aggregateType = firstAggregate.GetType();
            var aggregateTypeString = aggregateType.FullName;
            var aggregateTypeDisplayName = GetDisplayName(aggregateType);
            var maskSensitiveDataSerializerOptions = GetMaskSensitiveDataSerializerOptions();
            return (aggregate) => new AggregateDetails
            {
                AggregateType = aggregateTypeString,
                AggregateTypeDisplayName = aggregateTypeDisplayName,
                MaskedPayload = JsonSerializer.SerializeToDocument(aggregate, aggregateType, maskSensitiveDataSerializerOptions)
            };
        }
    }
}
