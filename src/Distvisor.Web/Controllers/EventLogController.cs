using Distvisor.App.Core.Dispatchers;
using Distvisor.App.EventLog.Qureies.GetAggregate;
using Distvisor.App.EventLog.Qureies.GetEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/s/event-log")]
    public class EventLogController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public EventLogController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<EventsListDto> GetEvents([FromQuery] GetEvents query, CancellationToken cancellationToken)
        {
            return await _dispatcher.DispatchAsync(query, cancellationToken);
        }

        [HttpGet("aggregates/{aggregateId}")]
        public async Task<AggregateDto> GetEvents([FromRoute] GetAggregate query, CancellationToken cancellationToken)
        {
            return await _dispatcher.DispatchAsync(query, cancellationToken);
        }
    }
}
