using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Distvisor.Web.Data.EventSourcing.Core
{
    public static class Extensions
    {
        public static void AddEventSourcing(this IServiceCollection services)
        {
            services.RegisterEventHandlers();
            services.AddSingleton<IEventStore, EventStore>();
            services.AddHostedService<EventSourcingBootstrap>();
        }

        private static void RegisterEventHandlers(this IServiceCollection services)
        {
            var handlerGenericType = typeof(IEventHandler<>);
            var assembly = handlerGenericType.Assembly;
            var handlers = assembly.GetTypes()
                .Where(x => !x.IsAbstract)
                .Select(x => new
                {
                    Implementation = x,
                    Interfaces = x.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition().Equals(handlerGenericType))
                })
                .Where(x => x.Interfaces.Any())
                .Where(x => !x.Interfaces.Contains(typeof(IEventHandler<object>)))
                .SelectMany(x => x.Interfaces.Select(i => new
                {
                    EventType = i.GetGenericArguments().Single(),
                    HandlerType = i,
                    ImplementationType = x.Implementation,
                }))
                .ToList();

            if (handlers.Count != handlers.Select(h => h.EventType).Distinct().Count())
            {
                throw new InvalidOperationException("Only one handler per event can be defined.");
            }

            services.AddSingleton<IEventHandler<object>>(sp =>
            {
                var mapping = handlers.ToDictionary(h => h.EventType, h => h.HandlerType);
                return new EventHandlerResolver(sp, mapping);
            });

            handlers.ForEach(h => services.AddSingleton(h.HandlerType, h.ImplementationType));

        }
    }
}
