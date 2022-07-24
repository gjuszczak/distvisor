using Distvisor.App.Common.Interfaces;
using Distvisor.App.Common.Models;
using Distvisor.App.Core.Queries;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.EventsLog.Qureies.GetEvents
{
    public class GetEvents : PaginatedQuery, IQuery<PaginatedList<EventsLogEntryDto>>
    {
    }

    public class GetEventsHandler : IQueryHandler<GetEvents, PaginatedList<EventsLogEntryDto>>
    {
        private readonly IEventsDbContext _context;

        public GetEventsHandler(IEventsDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<EventsLogEntryDto>> Handle(GetEvents request, CancellationToken cancellationToken)
        {
            var eventsQuery = _context.Events.OrderByDescending(x => x.TimeStamp);

            var eventEntities = await eventsQuery
                .ToPaginatedListAsync(request.FirstOffset, request.PageSize, cancellationToken);

            var dtos = eventEntities.Items
                .Select(entity => EventsLogEntryDto.FromEntity(entity, null, null, null))
                .ToList();

            return new PaginatedList<EventsLogEntryDto>(
                dtos, 
                eventEntities.TotalCount, 
                eventEntities.FirstOffset, 
                eventEntities.PageSize);
        }
    }
}
