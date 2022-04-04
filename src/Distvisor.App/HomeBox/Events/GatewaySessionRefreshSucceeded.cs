﻿using Distvisor.App.Common;
using Distvisor.App.Core.Events;
using Distvisor.App.HomeBox.Enums;
using Distvisor.App.HomeBox.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Events
{
    public class GatewaySessionRefreshSucceeded : Event
    {
        public GatewayToken Token { get; init; }

        public GatewaySessionRefreshSucceeded(GatewayToken token)
        {
            Token = token;
        }
    }

    public class GatewaySessionRefreshSucceededHandler : IEventHandler<GatewaySessionRefreshSucceeded>
    {
        private readonly IAppDbContext _appDbContext;
        public GatewaySessionRefreshSucceededHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Handle(GatewaySessionRefreshSucceeded @event, CancellationToken cancellationToken)
        {
            var entity = await _appDbContext.HomeboxGatewaySessions.FindAsync(new object[] { @event.AggregateId }, cancellationToken);
            if (entity != null)
            {
                entity.Token = @event.Token;
                entity.Status = GatewaySessionStatus.Open;
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
