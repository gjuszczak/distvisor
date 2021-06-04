using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/sec/[controller]")]
    public class RedirectionsController : ControllerBase
    {
        private readonly IRedirectionsService _redirections;

        public RedirectionsController(IRedirectionsService redirections)
        {
            _redirections = redirections;
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> RemoveRedirection(string name)
        {
            await _redirections.RemoveRedirectionAsync(name);
            return Ok();
        }

        [HttpGet]
        public Task<IEnumerable<RedirectionDetails>> ListRedirections()
        {
            return _redirections.ListRedirectionsAsync();
        }

        [HttpPost]
        public async Task<IActionResult> ConfigureRedirection(RedirectionDetails redirection)
        {
            await _redirections.ConfigureRedirectionAsync(redirection);
            return Ok();
        }
    }
}
