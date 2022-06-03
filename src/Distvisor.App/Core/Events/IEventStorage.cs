using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Events
{
    public interface IEventStorage
    {
        Task SaveAsync(EventEntity eventData, CancellationToken cancellationToken);

        Task<IEnumerable<EventEntity>> GetAsync(Type aggregateRootType, Guid aggregateId, bool useLastEventOnly, int fromVersion, CancellationToken cancellationToken);

        Task<IEnumerable<EventEntity>> GetToVersionAsync(Type aggregateRootType, Guid aggregateId, int version, CancellationToken cancellationToken);

        Task<IEnumerable<EventEntity>> GetToDateAsync(Type aggregateRootType, Guid aggregateId, DateTime versionedDate, CancellationToken cancellationToken);

        Task<IEnumerable<EventEntity>> GetBetweenDatesAsync(Type aggregateRootType, Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate, CancellationToken cancellationToken);
    }
}
