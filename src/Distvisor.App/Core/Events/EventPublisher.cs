using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, IEventPublishHelper> _eventPublishHelpers = new();

        public EventPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Publish(IEvent @event)
        {
            var eventType = @event.GetType();
            var eventPublishHelper = _eventPublishHelpers.GetOrAdd(eventType, CreateEventPublishHelper);
            eventPublishHelper.Publish(_serviceProvider, @event, default(CancellationToken)).GetAwaiter().GetResult();
        }

        public void Publish(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                Publish(@event);
            }
        }

        private static IEventPublishHelper CreateEventPublishHelper(Type eventType)
        {
            var dispatchHelperType = typeof(EventPublishHelper<>).MakeGenericType(eventType);
            return (IEventPublishHelper)Activator.CreateInstance(dispatchHelperType);
        }

        private interface IEventPublishHelper
        {
            Task Publish(IServiceProvider serviceProvider, IEvent @event, CancellationToken cancellationToken);
        }

        private class EventPublishHelper<TEvent> : IEventPublishHelper
            where TEvent : IEvent
        {
            public async Task Publish(IServiceProvider serviceProvider, IEvent @event, CancellationToken cancellationToken)
            {
                var handlers = serviceProvider.GetServices<IEventHandler<TEvent>>();
                foreach (var handler in handlers)
                {
                    await handler.Handle((TEvent)@event, cancellationToken);
                }
            }
        }
    }
}
