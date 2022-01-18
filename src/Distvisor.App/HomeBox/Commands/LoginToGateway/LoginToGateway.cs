using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Commands;
using Distvisor.App.HomeBox.Aggregates;
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
        private readonly IAggregateContext _context;
        private readonly IGatewayAuthenticationClient _gateway;

        public LoginToGatewayHandler(IAggregateContext context, IGatewayAuthenticationClient gateway)
        {
            _context = context;
            _gateway = gateway;
        }

        public async Task<Guid> Handle(LoginToGateway request, CancellationToken cancellationToken)
        {
            var authResult = await _gateway.LoginAsync(request.User, request.Password);
            var gatewaySession = new GatewaySession(request.Id, request.User, authResult.Token);
            _context.Add(gatewaySession);
            _context.Commit();

            return request.Id;
        }
    }
}
