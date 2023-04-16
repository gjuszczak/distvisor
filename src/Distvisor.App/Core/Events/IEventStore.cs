using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Events
{
    public interface IEventStore
	{
		Task<long> SaveAsync(IEvent @event, CancellationToken cancellationToken = default);

		Task<IEnumerable<IEvent>> GetAsync(Guid aggregateId, int fromVersion = -1, CancellationToken cancellationToken = default);
        
		Task<IEnumerable<IEvent>> GetAsync(long offsetId = 0, int batchSize = 1000, CancellationToken cancellationToken = default);

		Task<IEnumerable<IEvent>> GetAsync(Guid? aggregateId, int offset = 0, int batchSize = 1000, CancellationToken cancellationToken = default);

        Task<int> CountAsync(Guid? aggregateId, CancellationToken cancellationToken = default);
	}
}
