using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events.Core
{
    public class EventStoreBootstrap : IHostedService
    {
        private readonly IServiceProvider _provider;

        public EventStoreBootstrap(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _provider.CreateScope();
            var db = scope.ServiceProvider.GetService<EventStoreContext>();
            await db.Database.MigrateAsync(cancellationToken);
            
            // todo replay events
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
