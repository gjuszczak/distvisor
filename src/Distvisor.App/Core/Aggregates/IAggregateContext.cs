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

		Task<TAggregateRoot> GetAsync<TAggregateRoot>(Guid id, int? expectedVersion = null, CancellationToken cancellationToken = default)
			where TAggregateRoot : IAggregateRoot, new();

		Task<TAggregateRoot> GetToVersionAsync<TAggregateRoot>(Guid id, int version, CancellationToken cancellationToken = default)
			where TAggregateRoot : IAggregateRoot, new();

		Task<TAggregateRoot> GetToDateAsync<TAggregateRoot>(Guid id, DateTime versionedDate, CancellationToken cancellationToken = default)
			where TAggregateRoot : IAggregateRoot, new();

		Task CommitAsync(CancellationToken cancellationToken = default);
	}
}
