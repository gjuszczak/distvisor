using System;
using System.Collections.Generic;

namespace Distvisor.App.Core.Events
{
    public interface IEventStorage
    {
        void Save(EventEntity eventData);

        IEnumerable<EventEntity> Get(Type aggregateRootType, Guid aggregateId, bool useLastEventOnly, int fromVersion);

        IEnumerable<EventEntity> GetToVersion(Type aggregateRootType, Guid aggregateId, int version);

        IEnumerable<EventEntity> GetToDate(Type aggregateRootType, Guid aggregateId, DateTime versionedDate);

        IEnumerable<EventEntity> GetBetweenDates(Type aggregateRootType, Guid aggregateId, DateTime fromVersionedDate, DateTime toVersionedDate);
    }
}
