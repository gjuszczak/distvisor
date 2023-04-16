using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync(IEvent @event, CancellationToken cancellationToken);

        Task PublishAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken);
    }
}
