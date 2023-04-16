using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Events;
using Distvisor.App.Core.Exceptions;
using Distvisor.App.Core.Queries;
using Distvisor.App.EventLog.Services.DetailsProviding;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.EventLog.Qureies.GetAggregate
{
    public class GetAggregate : IQuery<AggregateDto>
    {
        public Guid AggregateId { get; set; }
    }

    public class GetAggregateHandler : IQueryHandler<GetAggregate, AggregateDto>
    {
        private readonly IEventStore _eventStore;
        private readonly IAggregateContext _aggregateContext;
        private readonly IAggregateDetailsProvider _aggregateDetailsProvider;

        public GetAggregateHandler(IEventStore eventStore, IAggregateContext aggregateContext, IAggregateDetailsProvider aggregateDetailsProvider)
        {
            _eventStore = eventStore;
            _aggregateContext = aggregateContext;
            _aggregateDetailsProvider = aggregateDetailsProvider;
        }

        public async Task<AggregateDto> Handle(GetAggregate request, CancellationToken cancellationToken)
        {
            var aggregateEvents = await _eventStore.GetAsync(request.AggregateId, batchSize: 1, cancellationToken: cancellationToken);
            if (!aggregateEvents.Any())
            {
                throw new AggregateNotFoundException(request.AggregateId);
            }
            var aggregateType = aggregateEvents.First().AggregateType;
            var getAggregateMethod = GetType()
                .GetMethod(nameof(GetAggregateAsync), BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(aggregateType);
            var aggregate = await (Task<IAggregateRoot>)getAggregateMethod.Invoke(this, new object[] { request.AggregateId, cancellationToken });
            var aggregateDetails = _aggregateDetailsProvider.GetDetails(aggregate);
            return AggregateDto.FromAggregate(aggregate, aggregateDetails);
        }

        private async Task<IAggregateRoot> GetAggregateAsync<TAggregateRoot>(Guid aggregateId, CancellationToken cancellationToken = default)
            where TAggregateRoot : IAggregateRoot, new()
            => await _aggregateContext.GetAsync<TAggregateRoot>(aggregateId, null, cancellationToken);
    }
}
