using System;

namespace Distvisor.App.Core.Exceptions
{
    public class ConcurrencyException : Exception
	{
		public ConcurrencyException(Guid aggregateId)
			: base($"Concurrency exception for aggregate {aggregateId}")
		{
			AggregateId = aggregateId;
		}

		public Guid AggregateId { get; set; }
	}
}
