using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Events
{
	public class EventStore : IEventStore
    {
		protected readonly IEventStorage _eventStorage;
		protected readonly IEventEntityBuilder _eventEntityBuilder;

		public EventStore(IEventStorage eventStorage, IEventEntityBuilder eventEntityBuilder)
		{
			_eventStorage = eventStorage;
			_eventEntityBuilder = eventEntityBuilder;
		}

		public virtual async Task<long> SaveAsync(IEvent @event, CancellationToken cancellationToken = default)
		{
			var eventEntity = _eventEntityBuilder.ToEventEntity(@event);
			return await _eventStorage.SaveAsync(eventEntity, cancellationToken);
		}

		public virtual async Task<IEnumerable<IEvent>> GetAsync(Guid aggregateId, int fromVersion = -1, CancellationToken cancellationToken = default)
        {
			var eventData = await _eventStorage.GetAsync(aggregateId, fromVersion, cancellationToken);
			var events = eventData.Select(_eventEntityBuilder.FromEventEntity).ToArray();
			return events;
        }

		public virtual async Task<IEnumerable<IEvent>> GetAsync(long offsetId = 0, int batchSize = 1000, CancellationToken cancellationToken = default)
        {
            var eventData = await _eventStorage.GetAsync(offsetId, batchSize, cancellationToken);
            var events = eventData.Select(_eventEntityBuilder.FromEventEntity).ToArray();
            return events;
        }

        public virtual async Task<IEnumerable<IEvent>> GetAsync(Guid? aggregateId, int offset = 0, int batchSize = 1000, CancellationToken cancellationToken = default)
        {
            var eventData = await _eventStorage.GetAsync(aggregateId, offset, batchSize, cancellationToken);
            var events = eventData.Select(_eventEntityBuilder.FromEventEntity).ToArray();
            return events;
        }

        public virtual Task<int> CountAsync(Guid? aggregateId, CancellationToken cancellationToken = default)
        {
            return _eventStorage.CountAsync(aggregateId, cancellationToken); ;
        }
    }
}
