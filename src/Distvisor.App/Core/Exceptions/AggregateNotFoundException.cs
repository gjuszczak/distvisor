using System;

namespace Distvisor.App.Core.Exceptions
{
    public class AggregateNotFoundException : Exception
	{
		public AggregateNotFoundException(Guid aggregateId, Type aggregateType)
			: base($"Aggregate {aggregateType.FullName}[id:{aggregateId}] not found")
		{
			AggregateId = aggregateId;
			AggregateType = aggregateType;
		}

		public Guid AggregateId { get; set; }
		public Type AggregateType { get; set; }
	}
}
