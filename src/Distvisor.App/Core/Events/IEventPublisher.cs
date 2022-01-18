using System.Collections.Generic;

namespace Distvisor.App.Core.Events
{
    public interface IEventPublisher
    {
        void Publish(IEvent @event);

        void Publish(IEnumerable<IEvent> events);
    }
}
