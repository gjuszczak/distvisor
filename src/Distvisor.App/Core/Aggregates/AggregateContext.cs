using Distvisor.App.Core.Exceptions;
using System;
using System.Collections.Generic;

namespace Distvisor.App.Core.Aggregates
{
	public class AggregateContext : IAggregateContext
    {
		private readonly IAggregateRepository _repository;
		private readonly Dictionary<Guid, TrackedAggregate> _trackedAggregates;

		public AggregateContext(IAggregateRepository repository)
		{
			_repository = repository;
			_trackedAggregates = new Dictionary<Guid, TrackedAggregate>();
		}

		public virtual void Add<TAggregateRoot>(TAggregateRoot aggregate)
			where TAggregateRoot : IAggregateRoot
		{
			if (!_trackedAggregates.ContainsKey(aggregate.AggregateId))
			{
				_trackedAggregates.Add(aggregate.AggregateId, new TrackedAggregate(aggregate));
			}
			else if (_trackedAggregates[aggregate.AggregateId] != (IAggregateRoot)aggregate)
			{
				throw new ConcurrencyException(aggregate.AggregateId);
			}
		}

		public virtual void Deatach(Guid id)
        {
			if (_trackedAggregates.ContainsKey(id))
            {
				_trackedAggregates.Remove(id);
            }
		}

		public virtual TAggregateRoot Get<TAggregateRoot>(Guid id, int? expectedVersion = null)
			where TAggregateRoot : IAggregateRoot
		{
			if (_trackedAggregates.ContainsKey(id))
			{
				var trackedAggregate = (TAggregateRoot)_trackedAggregates[id].Aggregate;
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
			where TAggregateRoot : IAggregateRoot
		{
			var aggregate = _repository.GetToVersion<TAggregateRoot>(id, version);
			return aggregate;
		}

		public virtual TAggregateRoot GetToDate<TAggregateRoot>(Guid id, DateTime versionedDate)
			where TAggregateRoot : IAggregateRoot
		{
			var aggregate = _repository.GetToDate<TAggregateRoot>(id, versionedDate);
			return aggregate;
		}

		public virtual void Commit()
		{
			foreach (var tracker in _trackedAggregates.Values)
			{
				_repository.Save(tracker.Aggregate, tracker.OriginalVersion);
			}
			_trackedAggregates.Clear();
		}

		private class TrackedAggregate
        {
			public IAggregateRoot Aggregate { get; init; }
			public int OriginalVersion { get; init; }

            public TrackedAggregate(IAggregateRoot aggregate)
            {
				Aggregate = aggregate;
				OriginalVersion = aggregate.Version;
            }
        }
	}
}
