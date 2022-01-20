using System;
using System.Buffers;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Distvisor.App.Core.Events
{
	public class EventEntityBuilder : IEventEntityBuilder
	{
		public virtual EventEntity ToEventEntity(IEvent @event, Type aggregateRootType)
		{
			return new EventEntity
			{
				EventId = @event.EventId,
				EventType = @event.GetType().FullName,
				AggregateId = @event.AggregateId,
				AggregateType = aggregateRootType.FullName,
				Version = @event.Version,
				CorrelationId = @event.CorrelationId,
				TimeStamp = @event.TimeStamp,
				Data = SerializeEventData(@event)
			};
		}

		public virtual IEvent FromEventEntity(EventEntity eventEntity)
		{
			using var stream = new MemoryStream();
			using var writer = new Utf8JsonWriter(stream);
			using var reader = new StreamReader(stream);
			eventEntity.Data.WriteTo(writer);
			writer.Flush();
			stream.Position = 0;
			var @event = (IEvent)JsonSerializer.Deserialize(reader.ReadToEnd(), Type.GetType(eventEntity.EventType));
			@event.EventId = eventEntity.EventId;
			@event.AggregateId = eventEntity.AggregateId;
			@event.Version = eventEntity.Version;
			@event.CorrelationId = eventEntity.CorrelationId;
			@event.TimeStamp = eventEntity.TimeStamp;
			return @event;
		}

		protected virtual JsonDocument SerializeEventData(IEvent @event)
        {
			var jsonString = JsonSerializer.Serialize(@event, @event.GetType());
			var jsonDocument = JsonDocument.Parse(jsonString);
			return jsonDocument;
        }
    }
}
