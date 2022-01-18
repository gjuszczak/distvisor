using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Commands;
using Distvisor.App.Core.Exceptions;
using Distvisor.App.HomeBox.Aggregates;
using Distvisor.App.HomeBox.Services.Gateway;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Commands.LoginToGateway
{
    public class SyncDevicesWithGateway : Command
    {
    }

    public class SyncDevicesWithGatewayHandler : ICommandHandler<SyncDevicesWithGateway>
    {
        private readonly IAggregateContext _context;
        private readonly IGatewayClient _gateway;

        public SyncDevicesWithGatewayHandler(IAggregateContext context, IGatewayClient gateway)
        {
            _context = context;
            _gateway = gateway;
        }

        public async Task<Guid> Handle(SyncDevicesWithGateway request, CancellationToken cancellationToken)
        {
            var gatewayDevices = await _gateway.GetDevicesAsync();
            

            return request.Id;
        }
    }
}
