using System;
using System.Collections.Generic;
using System.Linq;

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

		public virtual void Save<T>(IEvent @event)
		{
			Save(typeof(T), @event);
		}

		public virtual void Save(Type aggregateRootType, IEvent @event)
		{
			var eventEntity = _eventEntityBuilder.ToEventEntity(@event, aggregateRootType);
			_eventStorage.Save(eventEntity);
		}

		public virtual IEnumerable<IEvent> Get<T>(Guid aggregateId, bool useLastEventOnly = false, int fromVersion = -1)
		{
			return Get(typeof(T), aggregateId, useLastEventOnly, fromVersion).ToList();
		}

		public virtual IEnumerable<IEvent> Get(Type aggregateRootType, Guid aggregateId, bool useLastEventOnly = false, int fromVersion = -1)
        {
			var eventData = _eventStorage.Get(aggregateRootType, aggregateId, useLastEventOnly, fromVersion);
			var events = eventData.Select(e => _eventEntityBuilder.FromEventEntity(e)).ToArray();
			return events;
        }

		public virtual IEnumerable<IEvent> GetToVersion<T>(Guid aggregateId, int version)
		{
			return GetToVersion(typeof(T), aggregateId, version).ToList();
		}

		public virtual IEnumerable<IEvent> GetToVersion(Type aggregateRootType, Guid aggregateId, int version)
        {
			var eventData = _eventStorage.GetToVersion(aggregateRootType, aggregateId, version);
			var events = eventData.Select(e => _eventEntityBuilder.FromEventEntity(e)).ToArray();
			return events;
		}

		public virtual IEnumerable<IEvent> GetToDate<T>(Guid aggregateId, DateTime versionedDate)
		{
			return GetToDate(typeof(T), aggregateId, versionedDate).ToList();
		}

		public virtual IEnumerable<IEvent> GetToDate(Type aggregateRootType, Guid aggregateId, DateTime versionedDate)
        {
			var eventData = _eventStorage.GetToDate(aggregateRootType, aggregateId, versionedDate);
			var events = eventData.Select(e => _eventEntityBuilder.FromEventEntity(e)).ToArray();
			return events;
		}

		public virtual IEnumerable<IEvent> GetBetweenDates<T>(Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate)
		{
			return GetBetweenDates(typeof(T), aggregateId, fromVersionedDate, toVersionedDate).ToList();
		}

		public virtual IEnumerable<IEvent> GetBetweenDates(Type aggregateRootType, Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate)
        {
			var eventData = _eventStorage.GetBetweenDates(aggregateRootType, aggregateId, fromVersionedDate, toVersionedDate);
			var events = eventData.Select(e => _eventEntityBuilder.FromEventEntity(e)).ToArray();
			return events;
		}
	}
}
