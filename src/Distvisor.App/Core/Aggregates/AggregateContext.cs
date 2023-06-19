using Distvisor.App.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

		public virtual async Task<TAggregateRoot> GetAsync<TAggregateRoot>(Guid id, int? expectedVersion, CancellationToken cancellationToken = default)
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

			var aggregate = await _repository.GetAsync<TAggregateRoot>(id, cancellationToken: cancellationToken);
			if (expectedVersion != null && aggregate.Version != expectedVersion)
			{
				throw new ConcurrencyException(id);
			}
			Add(aggregate);

			return aggregate;
		}

		public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
		{
			foreach (var tracker in _aggregateTrackers.Values)
			{
				await tracker.SaveAsync(_repository, cancellationToken);
			}
			_aggregateTrackers.Clear();
		}

		private interface IAggregateTracker
        {
			IAggregateRoot Aggregate { get; }
			Task SaveAsync(IAggregateRepository repository, CancellationToken cancellationToken = default);
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

            public async Task SaveAsync(IAggregateRepository repository, CancellationToken cancellationToken = default)
            {
				await repository.SaveAsync((TAggregateRoot)Aggregate, OriginalVersion, cancellationToken);
			}
        }
	}
}
