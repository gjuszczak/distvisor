using Distvisor.App.Common.Interfaces;
using Distvisor.App.Core.Events;
using Distvisor.App.HomeBox.Entities;
using Distvisor.App.HomeBox.Enums;
using Distvisor.App.HomeBox.ValueObjects;
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
            _appDbContext.HomeboxGatewaySessions.Add(new GatewaySessionEntity
            {
                Id = @event.AggregateId,
                Username = @event.Username,
                Token = @event.Token,
                Status = GatewaySessionStatus.Open
            });
            await _appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
