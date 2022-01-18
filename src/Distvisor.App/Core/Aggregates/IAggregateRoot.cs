using Distvisor.App.Core.Events;
using System;
using System.Collections.Generic;

namespace Distvisor.App.Core.Aggregates
{
    public interface IAggregateRoot
    {
        Guid AggregateId { get; }
        int Version { get; }

        IEnumerable<IEvent> GetUncommittedChanges();
        void MarkChangesAsCommitted();
        void LoadFromHistory(IEnumerable<IEvent> history);
    }
}
