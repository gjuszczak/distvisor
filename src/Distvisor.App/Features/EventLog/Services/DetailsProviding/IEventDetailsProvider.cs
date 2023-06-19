using Distvisor.App.Core.Events;

namespace Distvisor.App.Features.EventLog.Services.DetailsProviding
{
    public interface IEventDetailsProvider
    {
        EventDetails GetDetails(IEvent @event);
    }
}