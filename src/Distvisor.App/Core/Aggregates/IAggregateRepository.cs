﻿using Distvisor.App.Core.Events;
using System;
using System.Collections.Generic;

namespace Distvisor.App.Core.Aggregates
{
    public interface IAggregateRepository
    {
        void Save<TAggregateRoot>(TAggregateRoot aggregate, int? expectedVersion = null)
            where TAggregateRoot : IAggregateRoot, new();

        TAggregateRoot Get<TAggregateRoot>(Guid aggregateId, IList<IEvent> events = null)
            where TAggregateRoot : IAggregateRoot, new();

        TAggregateRoot GetToVersion<TAggregateRoot>(Guid aggregateId, int version, IList<IEvent> events = null)
            where TAggregateRoot : IAggregateRoot, new();

        TAggregateRoot GetToDate<TAggregateRoot>(Guid aggregateId, DateTime versionedDate, IList<IEvent> events = null)
            where TAggregateRoot : IAggregateRoot, new();
    }
}
