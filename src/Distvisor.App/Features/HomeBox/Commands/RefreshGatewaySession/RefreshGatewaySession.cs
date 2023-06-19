using Distvisor.App.Core.Commands;
using Distvisor.App.Features.HomeBox.Services.Gateway;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.HomeBox.Commands.RefreshGatewaySession
{
    public class RefreshGatewaySession : Command
    {
        public Guid SessionId { get; set; }
    }

    public class RefreshGatewaySessionHandler : ICommandHandler<RefreshGatewaySession>
    {
        private readonly IGatewaySessionManager _sessionManager;

        public RefreshGatewaySessionHandler(IGatewaySessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task<Guid> Handle(RefreshGatewaySession request, CancellationToken cancellationToken)
        {
            await _sessionManager.RefreshSessionAsync(request.SessionId, cancellationToken);
            return request.Id;
        }
    }
}
