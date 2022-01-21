using System;

namespace Distvisor.App.Core.Aggregates
{
    public interface IAggregateContext
	{
		void Add<TAggregateRoot>(TAggregateRoot aggregate)
			where TAggregateRoot : IAggregateRoot, new();

		void Deatach(Guid id);

		TAggregateRoot Get<TAggregateRoot>(Guid id, int? expectedVersion = null)
			where TAggregateRoot : IAggregateRoot, new();

		TAggregateRoot GetToVersion<TAggregateRoot>(Guid id, int version)
			where TAggregateRoot : IAggregateRoot, new();

		TAggregateRoot GetToDate<TAggregateRoot>(Guid id, DateTime versionedDate)
			where TAggregateRoot : IAggregateRoot, new();

		void Commit();
	}
}
