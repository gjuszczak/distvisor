using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Events
{
    public interface IEventStore
	{
		Task SaveAsync<T>(IEvent @event, CancellationToken cancellationToken = default);

		Task SaveAsync(Type aggregateRootType, IEvent @event, CancellationToken cancellationToken = default);

		Task<IEnumerable<IEvent>> GetAsync<T>(Guid aggregateId, bool useLastEventOnly = false, int fromVersion = -1, CancellationToken cancellationToken = default);

		Task<IEnumerable<IEvent>> GetAsync(Type aggregateRootType, Guid aggregateId, bool useLastEventOnly = false, int fromVersion = -1, CancellationToken cancellationToken = default);

		Task<IEnumerable<IEvent>> GetToVersionAsync<T>(Guid aggregateId, int version, CancellationToken cancellationToken = default);

		Task<IEnumerable<IEvent>> GetToVersionAsync(Type aggregateRootType, Guid aggregateId, int version, CancellationToken cancellationToken = default);

		Task<IEnumerable<IEvent>> GetToDateAsync<T>(Guid aggregateId, DateTime versionedDate, CancellationToken cancellationToken = default);

		Task<IEnumerable<IEvent>> GetToDateAsync(Type aggregateRootType, Guid aggregateId, DateTime versionedDate, CancellationToken cancellationToken = default);

		Task<IEnumerable<IEvent>> GetBetweenDatesAsync<T>(Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate, CancellationToken cancellationToken = default);

		Task<IEnumerable<IEvent>> GetBetweenDatesAsync(Type aggregateRootType, Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate, CancellationToken cancellationToken = default);
	}
}
