using Distvisor.App.Core.Events;
using Distvisor.App.Core.Extensions;
using System.Text.Json;

namespace Distvisor.App.EventLog.Services.EventDetails
{
    public abstract class EventMaskingService<TEvent> : IEventMaskingService<TEvent>
        where TEvent : IEvent
    {
        public static readonly string MaskString = "*****";

        public virtual object Mask(JsonDocument eventPayload)
        {
            var deserializedPayload = eventPayload.Deserialize<TEvent>();
            return Mask(deserializedPayload);
        }

        protected abstract object Mask(TEvent @event);
    }
}
