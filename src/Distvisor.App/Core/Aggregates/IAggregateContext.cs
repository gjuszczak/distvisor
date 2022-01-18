using System;

namespace Distvisor.App.Core.Aggregates
{
    public interface IAggregateContext
	{
		void Add<TAggregateRoot>(TAggregateRoot aggregate)
			where TAggregateRoot : IAggregateRoot;

		void Deatach(Guid id);

		TAggregateRoot Get<TAggregateRoot>(Guid id, int? expectedVersion = null)
			where TAggregateRoot : IAggregateRoot;

		TAggregateRoot GetToVersion<TAggregateRoot>(Guid id, int version)
			where TAggregateRoot : IAggregateRoot;

		TAggregateRoot GetToDate<TAggregateRoot>(Guid id, DateTime versionedDate)
			where TAggregateRoot : IAggregateRoot;

		void Commit();
	}
}
