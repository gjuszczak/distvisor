using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.Events.Core
{
    public class EventStoreBootstrap : IHostedService
    {
        private readonly IServiceProvider _provider;
        private readonly string _readStorePath;
        public EventStoreBootstrap(IServiceProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            _readStorePath = configuration.GetConnectionString("ReadStore");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!File.Exists(_readStorePath))
            {
                using var scope = _provider.CreateScope();
                var eventStore = scope.ServiceProvider.GetService<IEventStore>();
                eventStore.ReplayEvents();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
