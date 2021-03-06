﻿using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Distvisor.Web.Data
{
    public static class Extensions
    {
        public static void AddEventSourcing(this IServiceCollection services, string eventsConnectionString, string readsConnectionString)
        {
            services.AddDbContext<EventStoreContext>(options => options.UseNpgsql(eventsConnectionString));
            services.AddDbContext<ReadStoreContext>(options => options.UseNpgsql(readsConnectionString));

            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<IReadStoreTransactionProvider, ReadStoreTransactionProvider>();
            services.RegisterEventHandlers();

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

            handlers.ForEach(h => services.AddScoped(h.HandlerType, h.ImplementationType));

            services.AddScoped<IEventHandlerResolver>(sp => new EventHandlerResolver(sp));
        }

        public static void DetachAllEntities(this DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            foreach (var entry in entries)
            {
                if (entry.Entity != null)
                {
                    entry.State = EntityState.Detached;
                }
            }
        }

        public static Guid GenerateIfEmpty(this Guid guid)
        {
            return guid == Guid.Empty ? Guid.NewGuid() : guid;
        }
    }
}
