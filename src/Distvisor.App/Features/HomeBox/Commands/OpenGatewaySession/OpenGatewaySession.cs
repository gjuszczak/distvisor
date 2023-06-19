using Distvisor.App.Core.Commands;
using Distvisor.App.Features.HomeBox.Services.Gateway;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.HomeBox.Commands.OpenGatewaySession
{
    public class OpenGatewaySession : Command
    {
        public string User { get; set; }
        public string Password { get; set; }
    }

    public class OpenGatewaySessionHandler : ICommandHandler<OpenGatewaySession>
    {
        private readonly IGatewaySessionManager _sessionManager;

        public OpenGatewaySessionHandler(IGatewaySessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task<Guid> Handle(OpenGatewaySession request, CancellationToken cancellationToken)
        {
            await _sessionManager.OpenGatewaySessionAsync(request.Id, request.User, request.Password, cancellationToken);
            return request.Id;
        }
    }
}
