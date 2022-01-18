using System;
using System.Collections.Generic;

namespace Distvisor.App.Core.Events
{
    public interface IEventStore
	{
		void Save<T>(IEvent @event);

		void Save(Type aggregateRootType, IEvent @event);

		IEnumerable<IEvent> Get<T>(Guid aggregateId, bool useLastEventOnly = false, int fromVersion = -1);

		IEnumerable<IEvent> Get(Type aggregateRootType, Guid aggregateId, bool useLastEventOnly = false, int fromVersion = -1);

		IEnumerable<IEvent> GetToVersion<T>(Guid aggregateId, int version);

		IEnumerable<IEvent> GetToVersion(Type aggregateRootType, Guid aggregateId, int version);

		IEnumerable<IEvent> GetToDate<T>(Guid aggregateId, DateTime versionedDate);

		IEnumerable<IEvent> GetToDate(Type aggregateRootType, Guid aggregateId, DateTime versionedDate);

		IEnumerable<IEvent> GetBetweenDates<T>(Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate);

		IEnumerable<IEvent> GetBetweenDates(Type aggregateRootType, Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate);
	}
}
