using Distvisor.App.Common.Interfaces;
using Distvisor.App.Common.Models;
using Distvisor.App.Core.Events;
using Distvisor.App.Core.Queries;
using Distvisor.App.EventLog.Services.EventDetails;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.EventLog.Qureies.GetEvents
{
    public class GetEvents : PaginatedQuery, IQuery<EventsListDto>
    {
        public Guid? AggregateId { get; set; }
    }

    public class GetEventsHandler : IQueryHandler<GetEvents, EventsListDto>
    {
        private readonly IEventsDbContext _context;
        private readonly IEventDetailsProvider _eventDetailsProvider;

        public GetEventsHandler(IEventsDbContext context, IEventDetailsProvider eventDetailsProvider)
        {
            _context = context;
            _eventDetailsProvider = eventDetailsProvider;
        }

        public async Task<EventsListDto> Handle(GetEvents request, CancellationToken cancellationToken)
        {
            var eventsQuery = _context.Events.AsQueryable();

            if (request.AggregateId.HasValue)
            {
                eventsQuery = eventsQuery.Where(ev => ev.AggregateId == request.AggregateId.Value);
            }

            var eventEntities = await eventsQuery
                .OrderByDescending(x => x.TimeStamp)
                .ToPaginatedListAsync(request.First, request.Rows, cancellationToken);

            var dtos = eventEntities.Items
                .Select(EventEntityToDto)
                .ToList();

            return new EventsListDto(
                dtos,
                eventEntities.TotalRecords,
                eventEntities.First,
                eventEntities.Rows,
                request.AggregateId);
        }

        private EventDto EventEntityToDto(EventEntity entity)
        {
            var details = _eventDetailsProvider.GetEventDetails(entity);
            return EventDto.FromEntity(entity, details);
        }
    }
}
