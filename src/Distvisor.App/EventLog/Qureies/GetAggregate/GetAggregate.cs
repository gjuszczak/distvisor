using Distvisor.App.Common.Interfaces;
using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Queries;
using Distvisor.App.EventLog.Services.EventDetails;
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
        private readonly IEventsDbContext _context;
        private readonly IAggregateContext _aggregateContext;
        private readonly IEventDetailsProvider _eventDetailsProvider;

        public GetAggregateHandler(IEventsDbContext context, IAggregateContext aggregateContext, IEventDetailsProvider eventDetailsProvider)
        {
            _context = context;
            _aggregateContext = aggregateContext;
            _eventDetailsProvider = eventDetailsProvider;
        }

        public async Task<AggregateDto> Handle(GetAggregate request, CancellationToken cancellationToken)
        {
            var aggregateTypeString = _context.Events.First(ev => ev.AggregateId == request.AggregateId)?.AggregateType;
            var aggregateType = Type.GetType(aggregateTypeString, true);
            var getAggregateMethod = GetType()
                .GetMethod(nameof(GetAggregateAsync), BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(aggregateType);
            var aggregate = await (Task<IAggregateRoot>)getAggregateMethod.Invoke(this, new object[] { request.AggregateId, cancellationToken });

            return new AggregateDto
            {
                AggregateId = aggregate.AggregateId,
                Version = aggregate.Version,
                AggregateType = aggregateTypeString,
            };
        }

        private async Task<IAggregateRoot> GetAggregateAsync<TAggregateRoot>(Guid aggregateId, CancellationToken cancellationToken = default)
            where TAggregateRoot : IAggregateRoot, new()
            => await _aggregateContext.GetAsync<TAggregateRoot>(aggregateId, null, cancellationToken);
    }
}
