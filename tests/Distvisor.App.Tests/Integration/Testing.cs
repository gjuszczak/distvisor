using Distvisor.App.Common;
using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Commands;
using Distvisor.App.Core.Events;
using Distvisor.App.Core.Queries;
using Distvisor.App.Core.Services;
using Distvisor.Infrastructure;
using Distvisor.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Reflection;
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
            services.AddMediatR(Assembly.GetAssembly(typeof(ICommand)));
            services.AddScoped<IAggregateContext, AggregateContext>();
            services.AddScoped<IAggregateRepository, AggregateRepository>();
            services.AddScoped<IAggregateProvider, AggreagateProvider>();
            services.AddScoped<IEventEntityBuilder, EventEntityBuilder>();
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<IEventStorage, SqlEventStorage>();
            services.AddScoped<IEventPublisher, EventPublisher>();
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IQueryBus, QueryBus>();
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

            var commandBus = scope.ServiceProvider.GetService<ICommandBus>();

            return await commandBus.ExecuteAsync(command);
        }

        public static async Task<TResult> Execute<TResult>(IQuery<TResult> query)
        {
            using var scope = _scopeFactory.CreateScope();

            var queryBus = scope.ServiceProvider.GetService<IQueryBus>();

            return await queryBus.ExecuteAsync(query);
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }
    }
}
