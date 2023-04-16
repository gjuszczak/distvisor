using Distvisor.App.Core.Events;

namespace Distvisor.App.EventLog.Services.DetailsProviding
{
    public interface IEventDetailsProvider
    {
        EventDetails GetDetails(IEvent @event);
    }
}