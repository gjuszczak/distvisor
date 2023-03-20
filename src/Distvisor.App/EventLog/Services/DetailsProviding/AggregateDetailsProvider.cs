using Distvisor.App.Core.Aggregates;
using System;
using System.Text.Json;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public class AggregateDetailsProvider : DetailsProvider<Type, IAggregateRoot, AggregateDetails>, IAggregateDetailsProvider
    {
        protected override Type GetDetailsFactoryKey(IAggregateRoot aggregate)
        {
            return aggregate.GetType();
        }

        protected override Func<IAggregateRoot, AggregateDetails> GetDetailsFactory(Type aggregateType)
        {
            var aggregateTypeString = aggregateType.ToString();
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
