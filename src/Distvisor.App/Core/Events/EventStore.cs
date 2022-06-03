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

		public virtual async Task SaveAsync<T>(IEvent @event, CancellationToken cancellationToken = default)
		{
			await SaveAsync(typeof(T), @event, cancellationToken);
		}

		public virtual async Task SaveAsync(Type aggregateRootType, IEvent @event, CancellationToken cancellationToken = default)
		{
			var eventEntity = _eventEntityBuilder.ToEventEntity(@event, aggregateRootType);
			await _eventStorage.SaveAsync(eventEntity, cancellationToken);
		}

		public virtual async Task<IEnumerable<IEvent>> GetAsync<T>(Guid aggregateId, bool useLastEventOnly = false, int fromVersion = -1, CancellationToken cancellationToken = default)
		{
			return await GetAsync(typeof(T), aggregateId, useLastEventOnly, fromVersion, cancellationToken);
		}

		public virtual async Task<IEnumerable<IEvent>> GetAsync(Type aggregateRootType, Guid aggregateId, bool useLastEventOnly = false, int fromVersion = -1, CancellationToken cancellationToken = default)
        {
			var eventData = await _eventStorage.GetAsync(aggregateRootType, aggregateId, useLastEventOnly, fromVersion, cancellationToken);
			var events = eventData.Select(e => _eventEntityBuilder.FromEventEntity(e)).ToArray();
			return events;
        }

		public virtual async Task<IEnumerable<IEvent>> GetToVersionAsync<T>(Guid aggregateId, int version, CancellationToken cancellationToken = default)
		{
			return await GetToVersionAsync(typeof(T), aggregateId, version, cancellationToken);
		}

		public virtual async Task<IEnumerable<IEvent>> GetToVersionAsync(Type aggregateRootType, Guid aggregateId, int version, CancellationToken cancellationToken = default)
        {
			var eventData = await _eventStorage.GetToVersionAsync(aggregateRootType, aggregateId, version, cancellationToken);
			var events = eventData.Select(e => _eventEntityBuilder.FromEventEntity(e)).ToArray();
			return events;
		}

		public virtual async Task<IEnumerable<IEvent>> GetToDateAsync<T>(Guid aggregateId, DateTime versionedDate, CancellationToken cancellationToken = default)
		{
			return await GetToDateAsync(typeof(T), aggregateId, versionedDate, cancellationToken);
		}

		public virtual async Task<IEnumerable<IEvent>> GetToDateAsync(Type aggregateRootType, Guid aggregateId, DateTime versionedDate, CancellationToken cancellationToken = default)
        {
			var eventData = await _eventStorage.GetToDateAsync(aggregateRootType, aggregateId, versionedDate, cancellationToken);
			var events = eventData.Select(e => _eventEntityBuilder.FromEventEntity(e)).ToArray();
			return events;
		}

		public virtual async Task<IEnumerable<IEvent>> GetBetweenDatesAsync<T>(Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate, CancellationToken cancellationToken = default)
		{
			return await GetBetweenDatesAsync(typeof(T), aggregateId, fromVersionedDate, toVersionedDate, cancellationToken);
		}

		public virtual async Task<IEnumerable<IEvent>> GetBetweenDatesAsync(Type aggregateRootType, Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate, CancellationToken cancellationToken = default)
        {
			var eventData = await _eventStorage.GetBetweenDatesAsync(aggregateRootType, aggregateId, fromVersionedDate, toVersionedDate, cancellationToken);
			var events = eventData.Select(e => _eventEntityBuilder.FromEventEntity(e)).ToArray();
			return events;
		}
	}
}
