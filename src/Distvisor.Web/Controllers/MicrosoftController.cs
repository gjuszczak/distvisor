using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MicrosoftController : ControllerBase
    {
        private readonly IMicrosoftService _microsoftService;

        public MicrosoftController(IMicrosoftService microsoftService)
        {
            _microsoftService = microsoftService;
        }

        [Authorize]
        [HttpGet("auth-url")]
        public MicrosoftAuthDto Authorize()
        {
            return new MicrosoftAuthDto
            {
                AuthUrl = _microsoftService.GetAuthorizeUrl()
            };
        }

        [HttpGet("auth-redirect")]
        public IActionResult Authorize(string code, string state)
        {

            return Ok();
        }
    }

    public class MicrosoftAuthDto
    {
        public string AuthUrl { get; set; }
    }
}
