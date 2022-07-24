using Distvisor.App.Common.Interfaces;
using Distvisor.App.Core.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Persistence
{
    public class SqlEventStorage : IEventStorage
    {
        private readonly IEventsDbContext _context;
        public SqlEventStorage(IEventsDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(EventEntity eventData, CancellationToken token)
        {
            await _context.Events.AddAsync(eventData, token);
            await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<EventEntity>> GetAsync(Type aggregateRootType, Guid aggregateId, bool useLastEventOnly, int fromVersion, CancellationToken token)
        {
            var query = _context.Events
                .Where(ev => ev.AggregateId == aggregateId && ev.Version > fromVersion);

            if (useLastEventOnly)
            {
                var last = await query.LastOrDefaultAsync(token);
                return last == null
                    ? new[] { last }
                    : Enumerable.Empty<EventEntity>();
            }

            return await query.ToArrayAsync(token);
        }

        public Task<IEnumerable<EventEntity>> GetBetweenDatesAsync(Type aggregateRootType, Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventEntity>> GetToDateAsync(Type aggregateRootType, Guid aggregateId, DateTime versionedDate, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventEntity>> GetToVersionAsync(Type aggregateRootType, Guid aggregateId, int version, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
