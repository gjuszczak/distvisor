using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events.Core
{
    public interface IEventHandler<T>
    {
        Task Handle(T payload);
    }

    public interface IEventHandlerResolver
    {
        IEventHandler<T> GetHandler<T>();
        IEventHandler<object> GetHandler(Type payloadType);
    }

    public class EventHandlerResolver : IEventHandlerResolver
    {
        private readonly IServiceProvider _provider;

        public EventHandlerResolver(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IEventHandler<T> GetHandler<T>()
        {
            var handlerType = typeof(IEventHandler<T>);
            var handler = (IEventHandler<T>)_provider.GetService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"Handler for {typeof(T).Name} not found.");
            }

            return handler;
        }

        public IEventHandler<object> GetHandler(Type payloadType)
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(payloadType);
            var handler = _provider.GetService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"Handler for {payloadType.Name} not found.");
            }

            return new GenericEventHandler(handler, handlerType);
        }

        private class GenericEventHandler : IEventHandler<object>
        {
            private readonly object _handler;
            private readonly MethodInfo _handleMethod;

            public GenericEventHandler(object handler, Type handlerType)
            {
                _handler = handler;
                _handleMethod = handlerType.GetMethod(nameof(Handle));
            }

            public async Task Handle(object payload)
            {
                await(Task)_handleMethod.Invoke(_handler, new[] { payload });
            }
        }
    }
}
