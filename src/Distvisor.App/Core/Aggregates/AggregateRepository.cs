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
        private readonly IAggregateProvider _aggregateFactory;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public AggregateRepository(
            IAggregateProvider aggregateFactory,
            IEventStore eventStore,
            IEventPublisher publisher,
            ICorrelationIdProvider correlationIdProvider)
        {
            _eventStore = eventStore;
            _publisher = publisher;
            _aggregateFactory = aggregateFactory;
            _correlationIdProvider = correlationIdProvider;
        }

        public virtual void Save<TAggregateRoot>(TAggregateRoot aggregate, int? expectedVersion = null)
            where TAggregateRoot : IAggregateRoot
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
            where TAggregateRoot : IAggregateRoot
        {
            IList<IEvent> theseEvents = events ?? _eventStore.Get<TAggregateRoot>(aggregateId).ToList();
            if (!theseEvents.Any())
            {
                throw new AggregateNotFoundException<TAggregateRoot>(aggregateId);
            }

            var duplicatedEvents =
                theseEvents.GroupBy(x => x.Version)
                    .Select(x => new { Version = x.Key, Total = x.Count() })
                    .FirstOrDefault(x => x.Total > 1);
            if (duplicatedEvents != null)
            {
                throw new DuplicateEventException<TAggregateRoot>(aggregateId, duplicatedEvents.Version);
            }

            var aggregate = _aggregateFactory.Create<TAggregateRoot>();
            aggregate.LoadFromHistory(theseEvents);
            return aggregate;
        }

        public virtual TAggregateRoot GetToVersion<TAggregateRoot>(Guid aggregateId, int version, IList<IEvent> events = null)
            where TAggregateRoot : IAggregateRoot
        {
            IList<IEvent> theseEvents = events ?? _eventStore.GetToVersion<TAggregateRoot>(aggregateId, version).ToList();
            if (!theseEvents.Any())
            {
                throw new AggregateNotFoundException<TAggregateRoot>(aggregateId);

            }

            var duplicatedEvents =
                theseEvents.GroupBy(x => x.Version)
                    .Select(x => new { Version = x.Key, Total = x.Count() })
                    .FirstOrDefault(x => x.Total > 1);
            if (duplicatedEvents != null)
            {
                throw new DuplicateEventException<TAggregateRoot>(aggregateId, duplicatedEvents.Version);
            }

            var aggregate = _aggregateFactory.Create<TAggregateRoot>();
            aggregate.LoadFromHistory(theseEvents);
            return aggregate;
        }

        public virtual TAggregateRoot GetToDate<TAggregateRoot>(Guid aggregateId, DateTime versionedDate, IList<IEvent> events = null)
            where TAggregateRoot : IAggregateRoot
        {
            IList<IEvent> theseEvents = events ?? _eventStore.GetToDate<TAggregateRoot>(aggregateId, versionedDate).ToList();
            if (!theseEvents.Any())
            {
                throw new AggregateNotFoundException<TAggregateRoot>(aggregateId);
            }

            var duplicatedEvents =
                theseEvents.GroupBy(x => x.Version)
                    .Select(x => new { Version = x.Key, Total = x.Count() })
                    .FirstOrDefault(x => x.Total > 1);
            if (duplicatedEvents != null)
            {
                throw new DuplicateEventException<TAggregateRoot>(aggregateId, duplicatedEvents.Version);
            }

            var aggregate = _aggregateFactory.Create<TAggregateRoot>();
            aggregate.LoadFromHistory(theseEvents);
            return aggregate;
        }
    }
}
