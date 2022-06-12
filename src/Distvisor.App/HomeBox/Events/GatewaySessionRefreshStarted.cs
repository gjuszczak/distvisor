using Distvisor.App.Common.Interfaces;
using Distvisor.App.Core.Events;
using Distvisor.App.HomeBox.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Events
{
    public class GatewaySessionRefreshStarted : Event
    {
        public DateTimeOffset Timeout { get; init; }

        public GatewaySessionRefreshStarted(DateTimeOffset timeout)
        {
            Timeout = timeout;
        }
    }

    public class GatewaySessionRefreshStartedHandler : IEventHandler<GatewaySessionRefreshStarted>
    {
        private readonly IAppDbContext _appDbContext;
        public GatewaySessionRefreshStartedHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Handle(GatewaySessionRefreshStarted @event, CancellationToken cancellationToken)
        {
            var entity = await _appDbContext.HomeboxGatewaySessions.FindAsync(new object[] { @event.AggregateId }, cancellationToken);
            if (entity != null)
            {
                entity.Status = GatewaySessionStatus.Refreshing;
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
