using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Data.EventSourcing.Core
{
    public class EventSourcingBootstrap : IHostedService
    {
        private readonly IEventStore _eventStore;
        public EventSourcingBootstrap(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _eventStore.ReplayEvents();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
