using Distvisor.App.Core.Events;
using System.Text.Json;

namespace Distvisor.App.EventLog.Services.EventDetails
{
    public interface IEventMaskingService
    {
        public object Mask(JsonDocument eventPayload);
    }

    public interface IEventMaskingService<TEvent> : IEventMaskingService 
        where TEvent : IEvent 
    {
    }
}
