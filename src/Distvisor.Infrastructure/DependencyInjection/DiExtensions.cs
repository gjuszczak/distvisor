using Distvisor.App.Admin.Services;
using Distvisor.App.Common.Interfaces;
using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Commands;
using Distvisor.App.Core.Dispatchers;
using Distvisor.App.Core.Events;
using Distvisor.App.Core.Queries;
using Distvisor.App.Core.Services;
using Distvisor.App.EventLog.Services.DetailsProviding;
using Distvisor.App.HomeBox.Services.Gateway;
using Distvisor.App.HomeBox.ValueObjects;
using Distvisor.Infrastructure.Persistence;
using Distvisor.Infrastructure.Persistence.App;
using Distvisor.Infrastructure.Persistence.Events;
using Distvisor.Infrastructure.Services.Admin;
using Distvisor.Infrastructure.Services.Common;
using Distvisor.Infrastructure.Services.HomeBox;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Distvisor.Infrastructure
{
    public static class DiExtensions
    {
        public static void AddDistvisor(this IServiceCollection services, IConfiguration config)
        {
            services.AddMemoryCache();

            services.AddSingleton<IEventEntityBuilder, EventEntityBuilder>();
            services.AddScoped<IAggregateContext, AggregateContext>();
            services.AddScoped<IAggregateRepository, AggregateRepository>();
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

            services.AddValidatorsFromAssemblyContaining<ICommand>();

            services.AddSingleton<IEventDetailsProvider, EventDetailsProvider>();
            services.AddSingleton<IAggregateDetailsProvider, AggregateDetailsProvider>();
            services.AddSingleton(SensitiveDataMaskConfiguration.Create()
                .MaskProperty((GatewayToken x) => x.AccessToken)
                .MaskProperty((GatewayToken x) => x.RefreshToken)
                .Build());

            services.AddScoped<IEventsReplayService, EventsReplayService>();
            services.AddScoped<IBackupService, BackupService>();
            services.AddScoped<IBackupFileService, BackupFileService>();

            services.Configure<GatewayConfiguration>(config.GetSection("HomeBox:Gateway"));
            services.AddHttpClient<IGatewayAuthenticationClient, GatewayAuthenticationClient>(c =>
            {
                c.BaseAddress = new Uri(config.GetValue<string>("HomeBox:Gateway:ApiUrl"));
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient<IGatewayClient, GatewayClient>(c =>
            {
                c.BaseAddress = new Uri(config.GetValue<string>("HomeBox:Gateway:ApiUrl"));
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddHttpClient<IFileHostingService, FileHostingService>(c =>
            {
                c.BaseAddress = new Uri(config.GetValue<string>("FileHosting:ApiUrl"));
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.Configure<FileHostingAuthConfiguration>(config.GetSection("FileHosting:Auth"));
            services.AddHttpClient<IFileHostingAuthService, FileHostingAuthService>(c =>
            {
                c.BaseAddress = new Uri(config.GetValue<string>("FileHosting:Auth:Instance"));
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}
