using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Events
{
    public class InMemoryEventStorage : IEventStorage
    {
        private readonly IDictionary<Guid, IList<EventEntity>> _inMemoryStorageDict = new Dictionary<Guid, IList<EventEntity>>();

        private readonly IList<EventEntity> _inMemoryStorageList = new List<EventEntity>();

        private long _idSeq = 1;

        public Task<long> SaveAsync(EventEntity eventData, CancellationToken cancellationToken)
        {
            if (!_inMemoryStorageDict.TryGetValue(eventData.AggregateId, out var list))
            {
                list = new List<EventEntity>();
                _inMemoryStorageDict.Add(eventData.AggregateId, list);
            }
            eventData.EventId = _idSeq++;
            list.Add(eventData);
            _inMemoryStorageList.Add(eventData);

            return Task.FromResult(eventData.EventId);
        }

        public Task<IEnumerable<EventEntity>> GetAsync(Guid aggregateId, int fromVersion, CancellationToken cancellationToken)
        {
            if (_inMemoryStorageDict.TryGetValue(aggregateId, out var list))
            {
                var entities = list.Where(x => x.Version > fromVersion).ToArray();
                return Task.FromResult(entities.AsEnumerable());
            }

            return Task.FromResult(Enumerable.Empty<EventEntity>());
        }

        public Task<IEnumerable<EventEntity>> GetAsync(long offsetId, int batchSize, CancellationToken cancellationToken)
        {
            var entities = _inMemoryStorageList.Where(x => x.EventId > offsetId).Take(batchSize).ToArray();
            return Task.FromResult(entities.AsEnumerable());
        }

        public Task<IEnumerable<EventEntity>> GetAsync(Guid? aggregateId, int offset, int batchSize, CancellationToken cancellationToken)
        {
            var entities = Enumerable.Empty<EventEntity>();

            if (!aggregateId.HasValue)
            {
                entities = _inMemoryStorageList;                    
            }
            else if (_inMemoryStorageDict.TryGetValue(aggregateId.Value, out var list))
            {
                entities = list;
            }

            return Task.FromResult(entities
                .OrderByDescending(x => x.EventId)
                .Skip(offset)
                .Take(batchSize)
                .ToArray()
                .AsEnumerable());
        }

        public Task<int> CountAsync(Guid? aggregateId, CancellationToken cancellationToken)
        {
            if (!aggregateId.HasValue)
            {
                return Task.FromResult(_inMemoryStorageList.Count);
            }
            else if (_inMemoryStorageDict.TryGetValue(aggregateId.Value, out var list))
            {
                return Task.FromResult(list.Count);
            }

            return Task.FromResult(0);
        }
    }
}
