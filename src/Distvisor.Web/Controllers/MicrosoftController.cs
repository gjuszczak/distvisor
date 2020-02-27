using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MicrosoftController : ControllerBase
    {
        private readonly IMicrosoftAuthService _authService;
        private readonly IMicrosoftAuthTokenStore _authTokenStore;
        private readonly IMicrosoftOneDriveService _oneDriveService;

        public MicrosoftController(IMicrosoftAuthService authService, IMicrosoftAuthTokenStore authTokenStore, IMicrosoftOneDriveService oneDriveService)
        {
            _authService = authService;
            _authTokenStore = authTokenStore;
            _oneDriveService = oneDriveService;
        }

        [Authorize]
        [HttpGet("auth-uri")]
        public MicrosoftAuthDto AuthUri()
        {
            return new MicrosoftAuthDto
            {
                AuthUri = _authService.GetAuthorizeUri()
            };
        }

        [HttpGet("auth-redirect")]
        public async Task<IActionResult> AuthRedirect(string code, string state)
        {
            var token = await _authService.ExchangeAuthCodeForBearerToken(code);
            var userId = Guid.Parse(state);
            await _authTokenStore.StoreUserTokenAsync(token, userId);
            return Redirect("/settings");
        }

        [Authorize]
        [HttpGet("backup")]
        public async Task Backup()
        {
            await _oneDriveService.BackupDb();
        }
    }

    public class MicrosoftAuthDto
    {
        public string AuthUri { get; set; }
    }
}
