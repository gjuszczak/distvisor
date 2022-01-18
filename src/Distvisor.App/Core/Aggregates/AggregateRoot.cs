using Distvisor.App.Core.Events;
using Distvisor.App.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Distvisor.App.Core.Aggregates
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        private ICollection<IEvent> _events;

        public Guid AggregateId { get; protected set; }
        public int Version { get; protected set; }

        protected AggregateRoot()
        {
            _events = new ReadOnlyCollection<IEvent>(new List<IEvent>());
            AggregateId = Guid.NewGuid();
        }

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            return _events;
        }

        public virtual void MarkChangesAsCommitted()
        {
            Version += _events.Count;
            _events = new ReadOnlyCollection<IEvent>(new List<IEvent>());
        }

        public virtual void LoadFromHistory(IEnumerable<IEvent> history)
        {
            var aggregateType = GetType();
            var aggregateId = history.FirstOrDefault()?.AggregateId;
            if (aggregateId.HasValue && AggregateId != aggregateId)
            {
                AggregateId = aggregateId.Value;
            }
            foreach (IEvent @event in history.OrderBy(e => e.Version))
            {
                if (@event.Version != Version + 1)
                {
                    throw new EventsOutOfOrderException(@event.AggregateId, aggregateType, Version + 1, @event.Version);
                }
                ApplyChange(@event, true);
            }
        }

        protected virtual void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, false);
        }

        private void ApplyChange(IEvent @event, bool isEventReplay)
        {
            ApplyChanges(new[] { @event }, isEventReplay);
        }

        protected virtual void ApplyChanges(IEnumerable<IEvent> events)
        {
            ApplyChanges(events, false);
        }

        private void ApplyChanges(IEnumerable<IEvent> events, bool isEventReplay)
        {
            IList<IEvent> changes = new List<IEvent>();
            try
            {
                foreach (IEvent @event in events)
                {
                    Apply(@event);
                    if (!isEventReplay)
                    {
                        changes.Add(@event);
                    }
                    else
                    {
                        AggregateId = @event.AggregateId;
                        Version++;
                    }
                }
            }
            finally
            {
                _events = new ReadOnlyCollection<IEvent>(_events.Concat(changes).ToList());
            }
        }

        protected abstract void Apply(IEvent @event);
    }
}
