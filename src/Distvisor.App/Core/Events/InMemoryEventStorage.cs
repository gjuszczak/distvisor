using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Events
{
    public class InMemoryEventStorage : IEventStorage
    {
        private readonly IDictionary<Guid, IList<EventEntity>> _inMemoryStorage =
            new Dictionary<Guid, IList<EventEntity>>();

        public Task SaveAsync(EventEntity eventData, CancellationToken cancellationToken)
        {
            if (!_inMemoryStorage.TryGetValue(eventData.AggregateId, out var list))
            {
                list = new List<EventEntity>();
                _inMemoryStorage.Add(eventData.AggregateId, list);
            }
            list.Add(eventData);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<EventEntity>> GetAsync(Type aggregateRootType, Guid aggregateId, bool useLastEventOnly, int fromVersion, CancellationToken cancellationToken)
        {
            if (_inMemoryStorage.TryGetValue(aggregateId, out var list))
            {
                var entities = list.Where(x => x.Version > fromVersion);
                var result = (entities.Any() && useLastEventOnly)
                    ? new[] { entities.Last() }
                    : entities.ToArray();
                return Task.FromResult(result.AsEnumerable());
            }

            return Task.FromResult(Enumerable.Empty<EventEntity>());
        }

        public Task<IEnumerable<EventEntity>> GetToVersionAsync(Type aggregateRootType, Guid aggregateId, int version, CancellationToken cancellationToken)
        {
            if (_inMemoryStorage.TryGetValue(aggregateId, out var list))
            {
                var result = list.Where(x => x.Version <= version).ToArray();
                return Task.FromResult(result.AsEnumerable());
            }

            return Task.FromResult(Enumerable.Empty<EventEntity>());
        }

        public Task<IEnumerable<EventEntity>> GetToDateAsync(Type aggregateRootType, Guid aggregateId, DateTime versionedDate, CancellationToken cancellationToken)
        {
            if (_inMemoryStorage.TryGetValue(aggregateId, out var list))
            {
                var result = list.Where(x => x.TimeStamp <= versionedDate).ToArray();
                return Task.FromResult(result.AsEnumerable());
            }

            return Task.FromResult(Enumerable.Empty<EventEntity>());
        }

        public Task<IEnumerable<EventEntity>> GetBetweenDatesAsync(Type aggregateRootType, Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate, CancellationToken cancellationToken)
        {
            if (_inMemoryStorage.TryGetValue(aggregateId, out var list))
            {
                var result = list.Where(x => x.TimeStamp >= fromVersionedDate && x.TimeStamp <= toVersionedDate).ToArray();
                return Task.FromResult(result.AsEnumerable());
            }

            return Task.FromResult(Enumerable.Empty<EventEntity>());
        }
    }
}
