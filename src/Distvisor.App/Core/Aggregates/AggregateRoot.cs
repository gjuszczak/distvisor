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
        private ICollection<IEvent> _events = new ReadOnlyCollection<IEvent>(new List<IEvent>());

        public Guid AggregateId { get; protected set; } = Guid.Empty;
        public int Version { get; protected set; } = 0;

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
            var aggregateId = history.FirstOrDefault()?.AggregateId;
            if (aggregateId.HasValue && AggregateId != aggregateId)
            {
                AggregateId = aggregateId.Value;
            }
            foreach (IEvent @event in history.OrderBy(e => e.Version))
            {
                if (@event.Version != Version + 1)
                {
                    throw new EventsOutOfOrderException(@event.AggregateId, this.GetType(), Version + 1, @event.Version);
                }
                Apply(@event);
                Version++;
            }
        }

        protected virtual void ApplyChange(IEvent @event)
        {
            Apply(@event);
            _events = new ReadOnlyCollection<IEvent>(_events.Concat(new[] { @event }).ToList());
        }

        protected abstract void Apply(IEvent @event);
    }
}
