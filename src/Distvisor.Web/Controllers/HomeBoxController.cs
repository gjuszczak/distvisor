using Distvisor.App.Core.Dispatchers;
using Distvisor.App.HomeBox.Commands;
using Distvisor.App.HomeBox.Queries.GetDevices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/s/home-box")]
    public class HomeBoxController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public HomeBoxController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("login-to-gateway")]
        public async Task LoginToGateway(LoginToGateway command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpPost("refresh-gateway-session")]
        public async Task RefreshGatewaySession(RefreshGatewaySession command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpPost("sync-devices-with-gateway")]
        public async Task SyncDevices(SyncDevicesWithGateway command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpGet("devices")]
        public async Task<IEnumerable<DeviceDto>> GetDevices(GetDevices query, CancellationToken cancellationToken)
        {
            return await _dispatcher.DispatchAsync(query, cancellationToken);
        }
    }
}
