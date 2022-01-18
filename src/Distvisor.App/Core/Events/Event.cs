using System;

namespace Distvisor.App.Core.Events
{
    public class Event : IEvent
    {
		public Guid EventId { get; set; }
		public Guid AggregateId { get; set; }
		public Guid CorrelationId { get; set; }
		public int Version { get; set; }
		public DateTimeOffset TimeStamp { get; set; }
	}
}
