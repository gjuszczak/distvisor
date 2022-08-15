using Distvisor.App.Core.Events;
using Distvisor.App.Core.Serialization;
using System.Text.Json;

namespace Distvisor.App.EventLog.Services.EventDetails
{
    public abstract class EventMaskingService<TEvent> : IEventMaskingService<TEvent>
        where TEvent : IEvent
    {
        public static readonly string MaskString = "*****";

        public virtual JsonDocument Mask(JsonDocument eventPayload)
        {
            var deserializedPayload = eventPayload.Deserialize<TEvent>(JsonDefaults.SerializerOptions);
            var maskedObject = Mask(deserializedPayload);
            var maskedPayload = maskedObject.SerializeToDocument(JsonDefaults.SerializerOptions);
            return maskedPayload;
        }

        protected abstract TEvent Mask(TEvent @event);
    }
}
