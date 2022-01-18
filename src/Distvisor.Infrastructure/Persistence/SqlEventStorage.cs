using Distvisor.App.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Distvisor.Infrastructure.Persistence
{
    public class SqlEventStorage : IEventStorage
    {
        private readonly AppDbContext _context;
        public SqlEventStorage(AppDbContext context)
        {
            _context = context;
        }

        public void Save(EventEntity eventData)
        {
            _context.Events.Add(eventData);
            _context.SaveChanges();
        }

        public IEnumerable<EventEntity> Get(Type aggregateRootType, Guid aggregateId, bool useLastEventOnly, int fromVersion)
        {
            var query = _context.Events
                .Where(ev => ev.AggregateId == aggregateId && ev.Version > fromVersion);

            if (useLastEventOnly)
            {
                var last = query.LastOrDefault();
                return last == null
                    ? new[] { last }
                    : Enumerable.Empty<EventEntity>();
            }

            return query.ToArray();
        }

        public IEnumerable<EventEntity> GetBetweenDates(Type aggregateRootType, Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventEntity> GetToDate(Type aggregateRootType, Guid aggregateId, DateTime versionedDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventEntity> GetToVersion(Type aggregateRootType, Guid aggregateId, int version)
        {
            throw new NotImplementedException();
        }
    }
}
