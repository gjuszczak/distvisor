using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Events
{
    public interface IEventStorage
    {
        Task<long> SaveAsync(EventEntity eventData, CancellationToken cancellationToken);

        Task<IEnumerable<EventEntity>> GetAsync(Guid aggregateId, int fromVersion, CancellationToken cancellationToken);

        Task<IEnumerable<EventEntity>> GetAsync(long offsetId, int batchSize, CancellationToken cancellationToken);

        Task<IEnumerable<EventEntity>> GetAsync(Guid? aggregateId, int offset, int batchSize, CancellationToken cancellationToken);

        Task<int> CountAsync(Guid? aggregateId, CancellationToken cancellationToken);
    }
}
