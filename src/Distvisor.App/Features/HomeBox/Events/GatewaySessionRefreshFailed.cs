﻿using Distvisor.App.Core.Events;
using Distvisor.App.Features.Common.Interfaces;
using Distvisor.App.Features.HomeBox.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.HomeBox.Events
{
    public class GatewaySessionRefreshFailed : Event
    {
    }

    public class GatewaySessionRefreshFailedHandler : IEventHandler<GatewaySessionRefreshFailed>
    {
        private readonly IAppDbContext _appDbContext;
        public GatewaySessionRefreshFailedHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Handle(GatewaySessionRefreshFailed @event, CancellationToken cancellationToken)
        {
            var entity = await _appDbContext.HomeboxGatewaySessions.FindAsync(new object[] { @event.AggregateId }, cancellationToken);
            if (entity != null)
            {
                entity.Status = GatewaySessionStatus.Closed;
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
