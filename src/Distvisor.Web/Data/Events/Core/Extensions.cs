using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Distvisor.Web.Data.Events.Core
{
    public static class Extensions
    {
        public static void AddEventStore(this IServiceCollection services)
        {
            services.AddSingleton<IDbProvider, DbProvider>();
            services.RegisterEventHandlers();
            services.AddScoped<IEventStore, EventStore>();
            services.AddHostedService<EventStoreBootstrap>();
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

            handlers.ForEach(h => services.AddScoped(h.HandlerType, h.ImplementationType));

            var mapping = handlers.ToDictionary(h => h.EventType, h => h.HandlerType);
            services.AddScoped<IEventHandler<object>>(sp => new EventHandlerResolver(sp, mapping));
        }
    }
}
