using Distvisor.Infrastructure.Services.Redirections;
using Microsoft.AspNetCore.Mvc;

namespace Distvisor.Api.Controllers
{
    [ApiController]
    [Route("api/redirect-to")]
    public class RedirectToController : ControllerBase
    {
        private readonly ICachedRedirectionsService _cachedRedirections;

        public RedirectToController(ICachedRedirectionsService cachedRedirections)
        {
            _cachedRedirections = cachedRedirections;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> RedirectTo(string name)
        {
            var redirection = await _cachedRedirections.GetByNameAsync(name);
            if (redirection == null)
            {
                return RedirectPermanent("/");
            }
            return RedirectPermanent(redirection.Url.ToString());
        }
    }
}
