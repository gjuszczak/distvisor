using Distvisor.Infrastructure.Persistence.App;
using Distvisor.Infrastructure.Persistence.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using Microsoft.EntityFrameworkCore;

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

            var eventsDbContext = scope.ServiceProvider.GetService<EventsDbContext>();
            await eventsDbContext.Database.MigrateAsync(cancellationToken);

            var appDbContext = scope.ServiceProvider.GetService<AppDbContext>();
            await appDbContext.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
