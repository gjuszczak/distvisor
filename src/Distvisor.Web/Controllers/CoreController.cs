using Distvisor.App.Core.Dispatchers;
using Distvisor.App.HomeBox.Commands.LoginToGateway;
using Distvisor.App.HomeBox.Queries.GetDevices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/core")]
    public class CoreController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public CoreController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("api-login")]
        public async Task ApiLogin(string username, string password)
        {
            await _dispatcher.DispatchAsync(new LoginToGateway
            {
                User = username,
                Password = password,
            });
        }

        [HttpPost("api-refresh")]
        public async Task ApiRefresh(Guid sessionId)
        {
            await _dispatcher.DispatchAsync(new RefreshGatewaySession
            {
                SessionId = sessionId
            });
        }

        [HttpPost("sync-devices")]
        public async Task SyncDevices()
        {
            await _dispatcher.DispatchAsync(new SyncDevicesWithGateway());
        }

        [HttpGet("devices")]
        public async Task GetDevices()
        {
            await _dispatcher.DispatchAsync(new GetDevices());
        }
    }
}
