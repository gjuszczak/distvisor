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
    [Route("api/[controller]")]
    public class RedirectionsController : ControllerBase
    {
        private readonly IRedirectionsService _redirections;

        public RedirectionsController(IRedirectionsService redirections)
        {
            _redirections = redirections;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> RedirectTo(string name)
        {
            if (Request.Cookies.ContainsKey("DoNotRedirect"))
            {
                return Redirect("/");
            }

            Response.Cookies.Append("DoNotRedirect", "", new CookieOptions { MaxAge = TimeSpan.FromMinutes(5) });

            var redirection = await _redirections.GetRedirectionAsync(name);
            if (redirection == null)
            {
                return Redirect("/");
            }
            return Redirect(redirection.Url.ToString());
        }

        [HttpDelete("{name}")]
        [Authorize]
        public async Task<IActionResult> RemoveRedirection(string name)
        {
            await _redirections.RemoveRedirectionAsync(name);
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public Task<IEnumerable<RedirectionDetails>> ListRedirections()
        {
            return _redirections.ListRedirectionsAsync();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ConfigureRedirection(RedirectionDetails redirection)
        {
            await _redirections.ConfigureRedirectionAsync(redirection);
            return Ok();
        }
    }
}
