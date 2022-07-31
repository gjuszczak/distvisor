using Distvisor.App.Common.Interfaces;
using Distvisor.App.Common.Models;
using Distvisor.App.Core.Events;
using Distvisor.App.Core.Queries;
using Distvisor.App.EventLog.Services.EventDetails;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.EventLog.Qureies.GetEvents
{
    public class GetEvents : PaginatedQuery, IQuery<PaginatedList<EventDto>>
    {
    }

    public class GetEventsHandler : IQueryHandler<GetEvents, PaginatedList<EventDto>>
    {
        private readonly IEventsDbContext _context;
        private readonly IEventDetailsProvider _eventDetailsProvider;

        public GetEventsHandler(IEventsDbContext context, IEventDetailsProvider eventDetailsProvider)
        {
            _context = context;
            _eventDetailsProvider = eventDetailsProvider;
        }

        public async Task<PaginatedList<EventDto>> Handle(GetEvents request, CancellationToken cancellationToken)
        {
            var eventsQuery = _context.Events.OrderByDescending(x => x.TimeStamp);

            var eventEntities = await eventsQuery
                .ToPaginatedListAsync(request.FirstOffset, request.PageSize, cancellationToken);

            var dtos = eventEntities.Items
                .Select(EventEntityToDto)
                .ToList();

            return new PaginatedList<EventDto>(
                dtos,
                eventEntities.TotalCount,
                eventEntities.FirstOffset,
                eventEntities.PageSize);
        }

        private EventDto EventEntityToDto(EventEntity entity)
        {
            var details = _eventDetailsProvider.GetEventDetails(entity);
            return EventDto.FromEntity(entity, details);
        }
    }
}
