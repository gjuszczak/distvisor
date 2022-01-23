using Distvisor.App.Common;
using Distvisor.App.Core.Events;
using Distvisor.App.HomeBox.Entities;
using Distvisor.App.HomeBox.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Events
{
    public class GatewaySessionOpened : Event
    {
        public string Username { get; init; }
        public GatewayToken Token { get; init; }

        public GatewaySessionOpened(string username, GatewayToken token)
        {
            Username = username;
            Token = token;
        }
    }

    public class GatewaySessionOpenedHandler : IEventHandler<GatewaySessionOpened>
    {
        private readonly IAppDbContext _appDbContext;
        public GatewaySessionOpenedHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Handle(GatewaySessionOpened @event, CancellationToken cancellationToken)
        {
            var existingSessionForUser = await _appDbContext.HomeboxGatewaySessions
                .Where(x => x.Username == @event.Username)
                .ToListAsync(cancellationToken);

            if (existingSessionForUser.Any())
            {
                _appDbContext.HomeboxGatewaySessions.RemoveRange(existingSessionForUser);
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }

            _appDbContext.HomeboxGatewaySessions.Add(new GatewaySessionEntity
            {
                Id = @event.AggregateId,
                Username = @event.Username
            });
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
