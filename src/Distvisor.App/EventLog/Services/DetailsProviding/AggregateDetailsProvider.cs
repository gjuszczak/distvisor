using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Serialization;
using System;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public class AggregateDetailsProvider : DetailsProvider<Type, IAggregateRoot, AggregateDetails>, IAggregateDetailsProvider
    {
        public AggregateDetailsProvider(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override Type GetDetailsFactoryKey(IAggregateRoot aggregate)
        {
            return aggregate.GetType();
        }

        protected override Func<IAggregateRoot, AggregateDetails> GetDetailsFactory(Type aggregateType)
        {
            var aggregateTypeString = aggregateType.ToString();
            var aggregateTypeDisplayName = GetDisplayName(aggregateType);
            var maskingService = GetMaskingService(aggregateType);
            return (aggregate) =>
            {
                var jsonDoc = aggregate.SerializeToDocument(aggregateType, JsonDefaults.SerializerOptions);
                return new AggregateDetails
                {
                    AggregateType = aggregateTypeString,
                    AggregateTypeDisplayName = aggregateTypeDisplayName,
                    MaskedPayload = maskingService?.Mask(jsonDoc) ?? jsonDoc
                };
            };
        }
    }
}
