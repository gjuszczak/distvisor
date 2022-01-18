using System;
using System.Buffers;
using System.Text.Json;

namespace Distvisor.App.Core.Events
{
	public class EventEntityBuilder : IEventEntityBuilder
	{
		public virtual EventEntity ToEventEntity(IEvent @event)
		{
			return new EventEntity
			{
				EventId = Guid.NewGuid(),
				EventType = @event.GetType().AssemblyQualifiedName,
				Data = SerializeEventData(@event)
			};
		}

		public virtual IEvent FromEventEntity(EventEntity eventEntity)
		{
			var bufferWriter = new ArrayBufferWriter<byte>();
			using var writer = new Utf8JsonWriter(bufferWriter);
			eventEntity.Data.WriteTo(writer);

			return (IEvent)JsonSerializer.Deserialize(bufferWriter.WrittenSpan, Type.GetType(eventEntity.EventType));
		}

		protected virtual JsonElement SerializeEventData(IEvent @event)
        {
			var jsonString = JsonSerializer.Serialize(@event);
			var jsonDocument = JsonDocument.Parse(jsonString);
			return jsonDocument.RootElement;
        }
	}
}
