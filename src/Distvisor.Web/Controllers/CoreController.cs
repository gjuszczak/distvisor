using Distvisor.App.Core.Commands;
using Distvisor.App.HomeBox.Commands.LoginToGateway;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/core")]
    public class CoreController : ControllerBase
    {
        private readonly ICommandBus _commandBus;

        public CoreController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        [HttpPost("api-login")]
        public async Task ApiLogin(string username, string password)
        {
            await _commandBus.ExecuteAsync(new LoginToGateway
            {
                User = username,
                Password = password,
            });
        }

        [HttpGet("devices")]
        public async Task GetDevices()
        {
            await _commandBus.ExecuteAsync(new SyncDevicesWithGateway());
        }
    }
}
