using System;

namespace Distvisor.App.Core.Exceptions
{
    public class AggregateMissingIdException : Exception
	{
		public AggregateMissingIdException(Type aggregateType, Type eventType)
			: base($"An event of type {eventType.FullName} cannot be saved, because it's source aggregate of type {aggregateType.FullName} has no id set")
		{
			AggregateType = aggregateType;
			EventType = eventType;
		}

		public Type AggregateType { get; set; }
		public Type EventType { get; set; }
	}
}
