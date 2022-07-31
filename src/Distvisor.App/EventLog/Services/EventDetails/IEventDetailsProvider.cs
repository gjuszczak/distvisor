using Distvisor.App.Core.Events;

namespace Distvisor.App.EventLog.Services.EventDetails
{
    public interface IEventDetailsProvider
    {
        EventDetails GetEventDetails(EventEntity eventEntity);
    }
}