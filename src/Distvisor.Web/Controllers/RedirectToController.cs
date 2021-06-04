using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/redirect-to")]
    public class RedirectToController : ControllerBase
    {
        private readonly IRedirectionsService _redirections;

        public RedirectToController(IRedirectionsService redirections)
        {
            _redirections = redirections;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> RedirectTo(string name)
        {
            var redirection = await _redirections.GetRedirectionAsync(name);
            if (redirection == null)
            {
                return RedirectPermanent("/");
            }
            return RedirectPermanent(redirection.Url.ToString());
        }
    }
}
