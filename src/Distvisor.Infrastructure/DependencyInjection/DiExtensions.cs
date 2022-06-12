using Distvisor.App.Common.Interfaces;
using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Commands;
using Distvisor.App.Core.Dispatchers;
using Distvisor.App.Core.Events;
using Distvisor.App.Core.Queries;
using Distvisor.App.Core.Services;
using Distvisor.App.HomeBox.Services.Gateway;
using Distvisor.Infrastructure.Persistence;
using Distvisor.Infrastructure.Persistence.App;
using Distvisor.Infrastructure.Persistence.Events;
using Distvisor.Infrastructure.Services.HomeBox;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Distvisor.Infrastructure
{
    public static class DiExtensions
    {
        public static void AddDistvisor(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IAggregateContext, AggregateContext>();
            services.AddScoped<IAggregateRepository, AggregateRepository>();
            services.AddScoped<IEventEntityBuilder, EventEntityBuilder>();
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<IEventPublisher, EventPublisher>();
            services.AddScoped<IDispatcher, Dispatcher>();
            services.AddScoped<IPipelineProvider, PipelineProvider>();
            services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();

            services.AddScoped<IGatewaySessionManager, GatewaySessionManager>();
            services.AddScoped<IGatewayAuthenticationPolicy, GatewayAuthenticationPolicy>();
            services.AddScoped<IGatewayDevicesSyncService, GatewayDevicesSyncService>();

            services.AddDbContext<EventsDbContext>(options =>
                options.UseNpgsql(
                    config.GetConnectionString("Distvisor"),
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "events")));
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    config.GetConnectionString("Distvisor"),
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "app")));

            services.AddScoped<IAuditDataEnricher, AuditDataEnricher>();
            services.AddScoped<IEventStorage, SqlEventStorage>();
            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

            services.Scan(selector =>
            {
                selector.FromAssemblyOf<ICommand>()
                    .AddClasses(filter =>
                    {
                        filter.AssignableToAny(
                            typeof(ICommandHandler<>),
                            typeof(IQueryHandler<,>),
                            typeof(IEventHandler<>),
                            typeof(IPipelineBehaviour<,>));
                    })
                    .AsImplementedInterfaces()
                    .WithScopedLifetime();
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.Configure<GatewayConfiguration>(config.GetSection("Ewelink"));
            services.AddHttpClient<IGatewayAuthenticationClient, GatewayAuthenticationClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(config.GetValue<string>("Ewelink:ApiUrl"));
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                });

            services.AddHttpClient<IGatewayClient, GatewayClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(config.GetValue<string>("Ewelink:ApiUrl"));
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                });
        }
    }
}
