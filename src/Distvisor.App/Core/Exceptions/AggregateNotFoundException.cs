using Distvisor.App.Core.Aggregates;
using System;

namespace Distvisor.App.Core.Exceptions
{
    public class AggregateNotFoundException<TAggregateRoot> : Exception
		where TAggregateRoot : IAggregateRoot
	{
		public AggregateNotFoundException(Guid aggregateId)
			: base($"Aggregate {typeof(TAggregateRoot).FullName}[id:{aggregateId}] not found")
		{
			AggregateId = aggregateId;
			AggregateType = typeof(TAggregateRoot);
		}

		public Guid AggregateId { get; set; }
		public Type AggregateType { get; set; }
	}
}
