using Distvisor.App.Core.Events;
using Distvisor.App.Core.Exceptions;
using Distvisor.App.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Aggregates
{
    public class AggregateRepository : IAggregateRepository
    {
        private readonly IEventStore _eventStore;
        private readonly IEventPublisher _publisher;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public AggregateRepository(
            IEventStore eventStore,
            IEventPublisher publisher,
            ICorrelationIdProvider correlationIdProvider)
        {
            _eventStore = eventStore;
            _publisher = publisher;
            _correlationIdProvider = correlationIdProvider;
        }

        public virtual async Task SaveAsync<TAggregateRoot>(TAggregateRoot aggregate, int? expectedVersion = null, CancellationToken cancellationToken = default)
            where TAggregateRoot : IAggregateRoot, new()
        {
            var uncommittedChanges = aggregate.GetUncommittedChanges();
            if (!uncommittedChanges.Any())
            {
                return;
            }

            if (expectedVersion != null)
            {
                var eventStoreResults = await _eventStore.GetAsync(aggregate.GetType(), aggregate.AggregateId, false, expectedVersion.Value, cancellationToken);
                if (eventStoreResults.Any())
                {
                    throw new ConcurrencyException(aggregate.AggregateId);
                }
            }

            var eventsToPublish = new List<IEvent>();
            int version = aggregate.Version;
            foreach (IEvent @event in uncommittedChanges)
            {
                if (@event.EventId == Guid.Empty)
                {
                    @event.EventId = Guid.NewGuid();
                }

                if (@event.AggregateId == Guid.Empty)
                {
                    if (aggregate.AggregateId == Guid.Empty)
                    {
                        throw new AggregateMissingIdException(aggregate.GetType(), @event.GetType());
                    }

                    @event.AggregateId = aggregate.AggregateId;
                }

                @event.Version = ++version;
                @event.TimeStamp = DateTimeOffset.UtcNow;
                @event.CorrelationId = _correlationIdProvider.GetCorrelationId();
                await _eventStore.SaveAsync(aggregate.GetType(), @event, cancellationToken);
                eventsToPublish.Add(@event);
            }

            aggregate.MarkChangesAsCommitted();
            foreach (IEvent @event in eventsToPublish)
            {
                _publisher.Publish(@event);
            }
        }

        public virtual async Task<TAggregateRoot> GetAsync<TAggregateRoot>(Guid aggregateId, IList<IEvent> events = null, CancellationToken cancellationToken = default)
            where TAggregateRoot : IAggregateRoot, new()
        {
            var aggregateEvents = events ?? await _eventStore.GetAsync<TAggregateRoot>(aggregateId, cancellationToken: cancellationToken);
            if (!aggregateEvents.Any())
            {
                throw new AggregateNotFoundException(aggregateId, typeof(TAggregateRoot));
            }

            var duplicatedEvents = aggregateEvents.GroupBy(x => x.Version)
                .Select(x => new { Version = x.Key, Total = x.Count() })
                .FirstOrDefault(x => x.Total > 1);
            if (duplicatedEvents != null)
            {
                throw new DuplicateEventException<TAggregateRoot>(aggregateId, duplicatedEvents.Version);
            }

            var aggregate = new TAggregateRoot();
            aggregate.LoadFromHistory(aggregateEvents);
            return aggregate;
        }

        public virtual async Task<TAggregateRoot> GetToVersionAsync<TAggregateRoot>(Guid aggregateId, int version, IList<IEvent> events = null, CancellationToken cancellationToken = default)
            where TAggregateRoot : IAggregateRoot, new()
        {
            var aggregateEvents = events ?? await _eventStore.GetToVersionAsync<TAggregateRoot>(aggregateId, version, cancellationToken);
            if (!aggregateEvents.Any())
            {
                throw new AggregateNotFoundException(aggregateId, typeof(TAggregateRoot));

            }

            var duplicatedEvents = aggregateEvents.GroupBy(x => x.Version)
                .Select(x => new { Version = x.Key, Total = x.Count() })
                .FirstOrDefault(x => x.Total > 1);
            if (duplicatedEvents != null)
            {
                throw new DuplicateEventException<TAggregateRoot>(aggregateId, duplicatedEvents.Version);
            }

            var aggregate = new TAggregateRoot();
            aggregate.LoadFromHistory(aggregateEvents);
            return aggregate;
        }

        public virtual async Task<TAggregateRoot> GetToDateAsync<TAggregateRoot>(Guid aggregateId, DateTime versionedDate, IList<IEvent> events = null, CancellationToken cancellationToken = default)
            where TAggregateRoot : IAggregateRoot, new()
        {
            var aggregateEvents = events ?? await _eventStore.GetToDateAsync<TAggregateRoot>(aggregateId, versionedDate, cancellationToken);
            if (!aggregateEvents.Any())
            {
                throw new AggregateNotFoundException(aggregateId, typeof(TAggregateRoot));
            }

            var duplicatedEvents = aggregateEvents.GroupBy(x => x.Version)
                .Select(x => new { Version = x.Key, Total = x.Count() })
                .FirstOrDefault(x => x.Total > 1);
            if (duplicatedEvents != null)
            {
                throw new DuplicateEventException<TAggregateRoot>(aggregateId, duplicatedEvents.Version);
            }

            var aggregate = new TAggregateRoot();
            aggregate.LoadFromHistory(aggregateEvents);
            return aggregate;
        }
    }
}
