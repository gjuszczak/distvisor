using Distvisor.App.Core.Dispatchers;
using Distvisor.App.Features.HomeBox.Commands.DeleteGatewaySession;
using Distvisor.App.Features.HomeBox.Commands.OpenGatewaySession;
using Distvisor.App.Features.HomeBox.Commands.RefreshGatewaySession;
using Distvisor.App.Features.HomeBox.Commands.SyncDevicesWithGateway;
using Distvisor.App.Features.HomeBox.Queries.GetDevices;
using Distvisor.App.Features.HomeBox.Queries.GetGatewaySessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/home-box")]
    public class HomeBoxController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public HomeBoxController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet("sessions")]
        public async Task<GatewaySessionsListDto> GetGatewaySessions([FromQuery] GetGatewaySessions query, CancellationToken cancellationToken)
        {
            return await _dispatcher.DispatchAsync(query, cancellationToken);
        }

        [HttpPost("sessions/open")]
        public async Task OpenGatewaySession(OpenGatewaySession command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpPost("sessions/refresh")]
        public async Task RefreshGatewaySession(RefreshGatewaySession command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpPost("sessions/delete")]
        public async Task DeleteGatewaySession(DeleteGatewaySession command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpGet("devices")]
        public async Task<DevicesListDto> GetDevices([FromQuery] GetDevices query, CancellationToken cancellationToken)
        {
            return await _dispatcher.DispatchAsync(query, cancellationToken);
        }

        [HttpPost("devices/sync-with-gateway")]
        public async Task SyncDevices(CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(new SyncDevicesWithGateway(), cancellationToken);
        }
    }
}
