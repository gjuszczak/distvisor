using Distvisor.App.Core.Events;
using Distvisor.App.Core.Exceptions;
using Distvisor.App.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public virtual void Save<TAggregateRoot>(TAggregateRoot aggregate, int? expectedVersion = null)
            where TAggregateRoot : IAggregateRoot, new()
        {
            IList<IEvent> uncommittedChanges = aggregate.GetUncommittedChanges().ToList();
            if (!uncommittedChanges.Any())
            {
                return;
            }

            if (expectedVersion != null)
            {
                IEnumerable<IEvent> eventStoreResults = _eventStore.Get(aggregate.GetType(), aggregate.AggregateId, false, expectedVersion.Value);
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
                _eventStore.Save(aggregate.GetType(), @event);
                eventsToPublish.Add(@event);
            }

            aggregate.MarkChangesAsCommitted();
            foreach (IEvent @event in eventsToPublish)
            {
                _publisher.Publish(@event);
            }
        }

        public virtual TAggregateRoot Get<TAggregateRoot>(Guid aggregateId, IList<IEvent> events = null)
            where TAggregateRoot : IAggregateRoot, new()
        {
            var aggregateEvents = events ?? _eventStore.Get<TAggregateRoot>(aggregateId).ToList();
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

        public virtual TAggregateRoot GetToVersion<TAggregateRoot>(Guid aggregateId, int version, IList<IEvent> events = null)
            where TAggregateRoot : IAggregateRoot, new()
        {
            var aggregateEvents = events ?? _eventStore.GetToVersion<TAggregateRoot>(aggregateId, version).ToList();
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

        public virtual TAggregateRoot GetToDate<TAggregateRoot>(Guid aggregateId, DateTime versionedDate, IList<IEvent> events = null)
            where TAggregateRoot : IAggregateRoot, new()
        {
            var aggregateEvents = events ?? _eventStore.GetToDate<TAggregateRoot>(aggregateId, versionedDate).ToList();
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
