using Distvisor.App.Core.Commands;
using Distvisor.App.Core.Queries;
using Distvisor.App.HomeBox.Commands.LoginToGateway;
using Distvisor.App.HomeBox.Queries.GetDevices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/core")]
    public class CoreController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public CoreController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpPost("api-login")]
        public async Task ApiLogin(string username, string password)
        {
            await _commandDispatcher.DispatchAsync(new LoginToGateway
            {
                User = username,
                Password = password,
            });
        }

        [HttpGet("devices")]
        public async Task GetDevices()
        {
            await _queryDispatcher.DispatchAsync(new GetDevices());
        }
    }
}
