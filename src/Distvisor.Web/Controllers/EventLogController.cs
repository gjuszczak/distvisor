using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/sec/[controller]")]
    public class EventLogController : ControllerBase
    {
        private readonly EventStoreContext _context;
        private readonly IEventLogToDtoMapper _mapper;

        public EventLogController(EventStoreContext context, IEventLogToDtoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IEnumerable<EventLogDto>> ListEvents()
        {
            var eventEntities = await _context.Events.OrderByDescending(x => x.PublishDateUtc).ToListAsync();
            return eventEntities.Select(x => _mapper.Map(x));
        }
    }

    public class EventLogDto
    {
        public int Id { get; set; }
        public DateTime PublishDateUtc { get; set; }
        public string PayloadType { get; set; }
        public string PayloadTypeDisplayName { get; set; }
        public string Status { get; set; }
        public object MaskedPayload { get; set; }
    }
}
