using Distvisor.App.Core.Aggregates;
using System;

namespace Distvisor.App.Core.Exceptions
{
    public class DuplicateEventException<TAggregateRoot> : Exception
		where TAggregateRoot : IAggregateRoot
	{
		public DuplicateEventException(Guid aggregateId, int version)
			: base($"Duplicated event for aggregate{typeof(TAggregateRoot).FullName}[id:{aggregateId}, v:{version}]")
		{
			AggregateId = aggregateId;
			AggregateType = typeof(TAggregateRoot);
			Version = version;
		}

		public Guid AggregateId { get; set; }
		public Type AggregateType { get; set; }
		public int Version { get; set; }
	}
}
