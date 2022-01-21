using Distvisor.App.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace Distvisor.App.Core.Aggregates
{
	public class AggregateContext : IAggregateContext
    {
		private readonly IAggregateRepository _repository;
		private readonly Dictionary<Guid, IAggregateTracker> _aggregateTrackers;

		public AggregateContext(IAggregateRepository repository)
		{
			_repository = repository;
			_aggregateTrackers = new Dictionary<Guid, IAggregateTracker>();
		}

		public virtual void Add<TAggregateRoot>(TAggregateRoot aggregate)
			where TAggregateRoot : IAggregateRoot, new()
		{
			if (!_aggregateTrackers.ContainsKey(aggregate.AggregateId))
			{
				_aggregateTrackers.Add(aggregate.AggregateId, new AggregateTracker<TAggregateRoot>(aggregate));
			}
			else if (_aggregateTrackers[aggregate.AggregateId] != (IAggregateRoot)aggregate)
			{
				throw new ConcurrencyException(aggregate.AggregateId);
			}
		}

		public virtual void Deatach(Guid id)
        {
			if (_aggregateTrackers.ContainsKey(id))
            {
				_aggregateTrackers.Remove(id);
            }
		}

		public virtual TAggregateRoot Get<TAggregateRoot>(Guid id, int? expectedVersion = null)
			where TAggregateRoot : IAggregateRoot, new()
		{
			if (_aggregateTrackers.ContainsKey(id))
			{
				var trackedAggregate = (TAggregateRoot)_aggregateTrackers[id].Aggregate;
				if (expectedVersion.HasValue && trackedAggregate.Version != expectedVersion.Value)
				{
					throw new ConcurrencyException(trackedAggregate.AggregateId);
				}
				return trackedAggregate;
			}

			var aggregate = _repository.Get<TAggregateRoot>(id);
			if (expectedVersion != null && aggregate.Version != expectedVersion)
			{
				throw new ConcurrencyException(id);
			}
			Add(aggregate);

			return aggregate;
		}

		public virtual TAggregateRoot GetToVersion<TAggregateRoot>(Guid id, int version)
			where TAggregateRoot : IAggregateRoot, new()
		{
			var aggregate = _repository.GetToVersion<TAggregateRoot>(id, version);
			return aggregate;
		}

		public virtual TAggregateRoot GetToDate<TAggregateRoot>(Guid id, DateTime versionedDate)
			where TAggregateRoot : IAggregateRoot, new()
		{
			var aggregate = _repository.GetToDate<TAggregateRoot>(id, versionedDate);
			return aggregate;
		}

		public virtual void Commit()
		{
			foreach (var tracker in _aggregateTrackers.Values)
			{
				tracker.Save(_repository);
			}
			_aggregateTrackers.Clear();
		}

		private interface IAggregateTracker
        {
			IAggregateRoot Aggregate { get; }
			void Save(IAggregateRepository repository);
        }

		private class AggregateTracker<TAggregateRoot> : IAggregateTracker
			where TAggregateRoot : IAggregateRoot, new()
		{
			public IAggregateRoot Aggregate { get; init; }
			public int OriginalVersion { get; init; }

            public AggregateTracker(IAggregateRoot aggregate)
            {
				Aggregate = aggregate;
				OriginalVersion = aggregate.Version;
            }

            public void Save(IAggregateRepository repository)
            {
				repository.Save((TAggregateRoot)Aggregate, OriginalVersion);
			}
        }
	}
}
