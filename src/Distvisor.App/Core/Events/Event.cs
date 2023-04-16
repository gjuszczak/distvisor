using System;
using System.Text.Json.Serialization;

namespace Distvisor.App.Core.Events
{
    public class Event : IEvent
    {
		[JsonIgnore]
		public long EventId { get; set; }

		[JsonIgnore]
		public Guid AggregateId { get; set; }

        [JsonIgnore]
        public Type AggregateType { get; set; }

        [JsonIgnore]
		public Guid CorrelationId { get; set; }

		[JsonIgnore]
		public int Version { get; set; }

		[JsonIgnore]
		public DateTimeOffset TimeStamp { get; set; }
	}
}
