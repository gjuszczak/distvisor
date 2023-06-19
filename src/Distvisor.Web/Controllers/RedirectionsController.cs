using Distvisor.App.Core.Dispatchers;
using Distvisor.App.Features.Redirections.Queries.GetRedirections;
using Distvisor.App.Features.Redirections.Commands.CreateRedirection;
using Distvisor.App.Features.Redirections.Commands.DeleteRedirection;
using Distvisor.App.Features.Redirections.Commands.EditRedirection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/redirections")]
    public class RedirectionsController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public RedirectionsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<RedirectionsListDto> GetRedirections([FromQuery] GetRedirections query, CancellationToken cancellationToken)
        {
            return await _dispatcher.DispatchAsync(query, cancellationToken);
        }

        [HttpPost]
        public async Task CreateRedirection(CreateRedirection command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpDelete]
        public async Task DeleteRedirection(DeleteRedirection command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpPatch]
        public async Task EditRedirection(EditRedirection command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }
    }
}
