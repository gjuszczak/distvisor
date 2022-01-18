using System;
using System.Collections.Generic;
using System.Linq;

namespace Distvisor.App.Core.Events
{
    public class InMemoryEventStorage : IEventStorage
    {
        private readonly IDictionary<Guid, IList<EventEntity>> _inMemoryStorage =
            new Dictionary<Guid, IList<EventEntity>>();

        public void Save(EventEntity eventData)
        {
            if (!_inMemoryStorage.TryGetValue(eventData.AggregateId, out var list))
            {
                list = new List<EventEntity>();
                _inMemoryStorage.Add(eventData.AggregateId, list);
            }
            list.Add(eventData);
        }

        public IEnumerable<EventEntity> Get(Type aggregateRootType, Guid aggregateId, bool useLastEventOnly, int fromVersion)
        {
            if (_inMemoryStorage.TryGetValue(aggregateId, out var list))
            {
                var result = list.Where(x => x.Version > fromVersion).ToArray();

                return (result.Length > 0 && useLastEventOnly)
                    ? new[] { result.Last() }
                    : result;
            }

            return Enumerable.Empty<EventEntity>();
        }

        public IEnumerable<EventEntity> GetToVersion(Type aggregateRootType, Guid aggregateId, int version)
        {
            if (_inMemoryStorage.TryGetValue(aggregateId, out var list))
            {
                return list.Where(x => x.Version <= version).ToArray();
            }

            return Enumerable.Empty<EventEntity>();
        }

        public IEnumerable<EventEntity> GetToDate(Type aggregateRootType, Guid aggregateId, DateTime versionedDate)
        {
            if (_inMemoryStorage.TryGetValue(aggregateId, out var list))
            {
                return list.Where(x => x.TimeStamp <= versionedDate).ToArray();
            }

            return Enumerable.Empty<EventEntity>();
        }

        public IEnumerable<EventEntity> GetBetweenDates(Type aggregateRootType, Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate)
        {
            if (_inMemoryStorage.TryGetValue(aggregateId, out var list))
            {
                return list.Where(x => x.TimeStamp >= fromVersionedDate && x.TimeStamp <= toVersionedDate).ToArray();
            }

            return Enumerable.Empty<EventEntity>();
        }
    }
}
