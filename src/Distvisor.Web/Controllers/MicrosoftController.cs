using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

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
        [HttpGet("auth-uri")]
        public MicrosoftAuthDto AuthUri()
        {
            return new MicrosoftAuthDto
            {
                AuthUri = _microsoftService.GetAuthorizeUri()
            };
        }

        [HttpGet("auth-redirect")]
        public async Task<IActionResult> AuthRedirect(string code, string state)
        {
            var token = await _microsoftService.ExchangeAuthCodeForBearerToken(code);
            var userId = Guid.Parse(state);
            await _microsoftService.StoreUserToken(userId, token);
            return Redirect("/settings");
        }

        [Authorize]
        [HttpGet("backup")]
        public Task Backup()
        {
            return _microsoftService.CreateUploadSession("/scheme.txt");
        }
    }

    public class MicrosoftAuthDto
    {
        public string AuthUri { get; set; }
    }
}
