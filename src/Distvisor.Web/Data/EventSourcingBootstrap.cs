using Distvisor.Infrastructure;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Data
{
    public class EventSourcingBootstrap : IHostedService
    {
        private readonly IServiceProvider _provider;

        public EventSourcingBootstrap(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _provider.CreateScope();
            var eventsDb = scope.ServiceProvider.GetService<EventStoreContext>();
            await eventsDb.Database.MigrateAsync(cancellationToken);

            var readsDb = scope.ServiceProvider.GetService<ReadStoreContext>();
            await readsDb.Database.MigrateAsync(cancellationToken);

            var testDb = scope.ServiceProvider.GetService<AppDbContext>();
            await testDb.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
