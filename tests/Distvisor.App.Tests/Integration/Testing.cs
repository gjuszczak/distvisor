﻿using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Commands;
using Distvisor.App.Core.Dispatchers;
using Distvisor.App.Core.Events;
using Distvisor.App.Core.Queries;
using Distvisor.App.Core.Services;
using Distvisor.App.Features.Common.Interfaces;
using Distvisor.Infrastructure.Persistence;
using Distvisor.Infrastructure.Persistence.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Distvisor.App.Tests.Integration
{
    [SetUpFixture]
    public class Testing
    {
        private static IServiceScopeFactory _scopeFactory;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var services = new ServiceCollection();
            services.AddScoped<IAggregateContext, AggregateContext>();
            services.AddScoped<IAggregateRepository, AggregateRepository>();
            services.AddSingleton<IEventEntityBuilder, EventEntityBuilder>();
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<IEventStorage, SqlEventStorage>();
            services.AddScoped<IEventPublisher, EventPublisher>();
            services.AddScoped<IDispatcher, Dispatcher>();
            services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql("Host=localhost;Database=TestsDistvisor;Username=postgres;Password=mysecretpassword"));
            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
            using var scope = _scopeFactory.CreateScope();
            scope.ServiceProvider.GetService<AppDbContext>().Database.Migrate();
        }

        public static async Task<Guid> Execute(ICommand command)
        {
            using var scope = _scopeFactory.CreateScope();

            var commands = scope.ServiceProvider.GetService<IDispatcher>();

            return await commands.DispatchAsync(command);
        }

        public static async Task<TResult> Execute<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
        {
            using var scope = _scopeFactory.CreateScope();

            var queries = scope.ServiceProvider.GetService<IDispatcher>();

            return await queries.DispatchAsync(query);
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }
    }
}
