using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Commands;
using Distvisor.App.HomeBox.Aggregates;
using Distvisor.App.HomeBox.Services.Gateway;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Commands.LoginToGateway
{
    public class RefreshGatewaySession : Command
    {
        public Guid SessionId { get; set; }
    }

    public class RefreshGatewaySessionHandler : ICommandHandler<RefreshGatewaySession>
    {
        private readonly IAggregateContext _context;
        private readonly IGatewayAuthenticationClient _gateway;

        public RefreshGatewaySessionHandler(IAggregateContext context, IGatewayAuthenticationClient gateway)
        {
            _context = context;
            _gateway = gateway;
        }

        public async Task<Guid> Handle(RefreshGatewaySession request, CancellationToken cancellationToken)
        {
            var session = _context.Get<GatewaySession>(request.SessionId);
            session.BeginRefresh();
            _context.Commit();
            try
            {
                var refreshResult = await _gateway.RefreshSessionAsync(session.Token.RefreshToken);
                session.RefreshSucceed(refreshResult.Token);
            }
            catch
            {
                session.RefreshFailed();
            }
            _context.Commit();
            return request.Id;
        }
    }
}
