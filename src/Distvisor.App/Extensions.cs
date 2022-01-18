using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Commands;
using Distvisor.App.Core.Events;
using Distvisor.App.Core.Queries;
using Distvisor.App.Core.Services;
using Distvisor.App.HomeBox.Services.Gateway;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Distvisor.App
{
    public static class Extensions
    {
        public static void AddDistvisorApp(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetAssembly(typeof(ICommand)));
            services.AddScoped<IAggregateContext, AggregateContext>();
            services.AddScoped<IAggregateRepository, AggregateRepository>();
            services.AddScoped<IAggregateProvider, AggreagateProvider>();
            services.AddScoped<IEventEntityBuilder, EventEntityBuilder>();
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<IEventStorage, InMemoryEventStorage>();
            services.AddScoped<IEventPublisher, EventPublisher>();
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IQueryBus, QueryBus>();
            services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();

            services.AddScoped<IGatewaySessionManager, GatewaySessionManager>();
            services.AddScoped<IGatewayAuthenticationPolicy, GatewayAuthenticationPolicy>();
        }
    }
}
