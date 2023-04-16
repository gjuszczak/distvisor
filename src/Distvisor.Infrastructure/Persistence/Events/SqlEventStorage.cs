using Distvisor.App.Core.Events;
using Distvisor.Infrastructure.Persistence.Events;
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
        private readonly EventsDbContext _context;
        public SqlEventStorage(EventsDbContext context)
        {
            _context = context;
        }

        public async Task<long> SaveAsync(EventEntity eventData, CancellationToken token)
        {
            await _context.Events.AddAsync(eventData, token);
            await _context.SaveChangesAsync(token);
            return eventData.EventId;
        }

        public async Task<IEnumerable<EventEntity>> GetAsync(Guid aggregateId, int fromVersion, CancellationToken token)
        {
            var entities = await _context.Events
                .Where(ev => ev.AggregateId == aggregateId && ev.Version > fromVersion)
                .ToArrayAsync(token);
            return entities;
        }

        public async Task<IEnumerable<EventEntity>> GetAsync(long offsetId, int batchSize, CancellationToken cancellationToken)
        {
            var entities = await _context.Events
                .AsNoTracking()
                .Where(ev => ev.EventId > offsetId)
                .OrderBy(ev => ev.EventId)
                .Take(batchSize)
                .ToArrayAsync(cancellationToken);
            return entities;
        }

        public async Task<IEnumerable<EventEntity>> GetAsync(Guid? aggregateId, int offset, int batchSize, CancellationToken cancellationToken)
        {
            var query = _context.Events.AsQueryable();

            if (aggregateId.HasValue)
            {
                query = query.Where(x => x.AggregateId == aggregateId.Value);
            }

            return await query
                .OrderByDescending(x => x.EventId)
                .Skip(offset)
                .Take(batchSize)
                .ToArrayAsync(cancellationToken);
        }

        public async Task<int> CountAsync(Guid? aggregateId, CancellationToken cancellationToken)
        {
            var query = _context.Events.AsQueryable();

            if (aggregateId.HasValue)
            {
                query = query.Where(x => x.AggregateId == aggregateId.Value);
            }

            var count = await query.LongCountAsync(cancellationToken: cancellationToken);

            return count > int.MaxValue
                ? int.MaxValue
                : (int)count;
        }
    }
}
