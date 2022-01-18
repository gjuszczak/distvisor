using MediatR;
using System.Collections.Generic;

namespace Distvisor.App.Core.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IMediator _mediator;
        public EventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Publish(IEvent @event)
        {
            _mediator.Publish(@event).GetAwaiter().GetResult();
        }

        public void Publish(IEnumerable<IEvent> events)
        {
            foreach(var @event in events)
            {
                Publish(@event);
            }
        }
    }
}
