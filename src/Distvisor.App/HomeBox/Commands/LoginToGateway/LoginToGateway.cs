using Distvisor.App.Core.Commands;
using Distvisor.App.HomeBox.Services.Gateway;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Commands.LoginToGateway
{
    public class LoginToGateway : Command
    {
        public string User { get; set; }
        public string Password { get; set; }
    }

    public class LoginToGatewayHandler : ICommandHandler<LoginToGateway>
    {
        private readonly IGatewaySessionManager _sessionManager;

        public LoginToGatewayHandler(IGatewaySessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task<Guid> Handle(LoginToGateway request, CancellationToken cancellationToken)
        {
            await _sessionManager.OpenGatewaySession(request.Id, request.User, request.Password);
            return request.Id;
        }
    }
}
