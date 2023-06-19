using Distvisor.App.Core.Commands;
using Distvisor.App.Features.HomeBox.Services.Gateway;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.HomeBox.Commands.DeleteGatewaySession
{
    public class DeleteGatewaySession : Command
    {
        public Guid SessionId { get; set; }
    }

    public class DeleteGatewaySessionHandler : ICommandHandler<DeleteGatewaySession>
    {
        private readonly IGatewaySessionManager _sessionManager;

        public DeleteGatewaySessionHandler(IGatewaySessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task<Guid> Handle(DeleteGatewaySession request, CancellationToken cancellationToken)
        {
            await _sessionManager.DeleteSessionAsync(request.SessionId, cancellationToken);
            return request.Id;
        }
    }
}
