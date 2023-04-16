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
        private readonly Dictionary<Type, Action<object>> _eventHandlers = new();
        private readonly List<IEvent> _events = new();

        public Guid AggregateId { get; protected set; } = Guid.Empty;
        public int Version { get; protected set; } = 0;

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            return new ReadOnlyCollection<IEvent>(_events);
        }

        public void MarkChangesAsCommitted()
        {
            Version += _events.Count;
            _events.Clear();
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            var aggregateId = history.FirstOrDefault()?.AggregateId;
            if (!aggregateId.HasValue)
            {
                return;
            }
            if (AggregateId != aggregateId)
            {
                AggregateId = aggregateId.Value;
            }
            foreach (IEvent @event in history.OrderBy(e => e.Version))
            {
                if (@event.Version != Version + 1)
                {
                    throw new EventsOutOfOrderException(@event.AggregateId, GetType(), Version + 1, @event.Version);
                }
                Apply(@event);
                Version++;
            }
        }

        protected void RegisterEventHandler<TEvent>(Action<TEvent> handler)
            where TEvent : IEvent
        {
            _eventHandlers.Add(typeof(TEvent), x => handler((TEvent)x));
        }

        protected void ApplyEvent(IEvent @event)
        {
            Apply(@event);
            _events.Add(@event);
        }

        private void Apply(IEvent @event)
        {
            var eventType = @event.GetType();
            if (!_eventHandlers.TryGetValue(eventType, out var handler))
                throw new ArgumentException($"'{eventType.FullName}' is not supported by aggregate {GetType().FullName}", nameof(@event));

            handler(@event);
        }
    }
}
