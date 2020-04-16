using System;
using System.Collections.Generic;

namespace Distvisor.Web.Data.Events.Core
{
    public interface IEventHandler<T>
    {
        void Handle(T payload);
    }

    public class EventHandlerResolver : IEventHandler<object>
    {
        private readonly IServiceProvider _provider;
        private readonly Dictionary<Type, Type> _handlers;

        public EventHandlerResolver(IServiceProvider provider, Dictionary<Type, Type> handlers)
        {
            _provider = provider;
            _handlers = handlers;
        }

        public void Handle(object payload)
        {
            var payloadType = payload.GetType();
            if (_handlers.TryGetValue(payloadType, out Type handlerType))
            {
                var mi = handlerType.GetMethod(nameof(Handle));
                var handler = _provider.GetService(handlerType);
                mi.Invoke(handler, new[] { payload });
            }
            else
            {
                throw new InvalidOperationException($"Handler for {payloadType.Name} not found.");
            }
        }
    }
}
