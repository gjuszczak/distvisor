using Distvisor.App.Core.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Aggregates
{
    public interface IAggregateRepository
    {
        Task SaveAsync<TAggregateRoot>(TAggregateRoot aggregate, int? expectedVersion = null, CancellationToken cancellationToken = default)
            where TAggregateRoot : IAggregateRoot, new();

        Task<TAggregateRoot> GetAsync<TAggregateRoot>(Guid aggregateId, IList<IEvent> events = null, CancellationToken cancellationToken = default)
            where TAggregateRoot : IAggregateRoot, new();
    }
}
