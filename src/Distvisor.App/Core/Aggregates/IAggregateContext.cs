using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Aggregates
{
    public interface IAggregateContext
	{
		void Add<TAggregateRoot>(TAggregateRoot aggregate)
			where TAggregateRoot : IAggregateRoot, new();

		void Deatach(Guid id);

		Task<TAggregateRoot> GetAsync<TAggregateRoot>(Guid id, CancellationToken cancellationToken = default)
			where TAggregateRoot : IAggregateRoot, new()
			=> GetAsync<TAggregateRoot>(id, null, cancellationToken: cancellationToken);

		Task<TAggregateRoot> GetAsync<TAggregateRoot>(Guid id, int? expectedVersion, CancellationToken cancellationToken = default)
			where TAggregateRoot : IAggregateRoot, new();

        Task CommitAsync(CancellationToken cancellationToken = default);
	}
}
