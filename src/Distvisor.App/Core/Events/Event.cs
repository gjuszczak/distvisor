using System;
using System.Text.Json.Serialization;

namespace Distvisor.App.Core.Events
{
    public class Event : IEvent
    {
		[JsonIgnore]
		public Guid EventId { get; set; }

		[JsonIgnore]
		public Guid AggregateId { get; set; }

		[JsonIgnore]
		public Guid CorrelationId { get; set; }

		[JsonIgnore]
		public int Version { get; set; }

		[JsonIgnore]
		public DateTimeOffset TimeStamp { get; set; }
	}
}
