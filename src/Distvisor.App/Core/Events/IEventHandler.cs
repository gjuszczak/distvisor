using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Events
{
    public interface IEventHandler<in TEvent>
        where TEvent : IEvent
    {
        Task Handle(TEvent @event, CancellationToken cancellationToken);
    }
}
