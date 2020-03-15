using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IDistvisorAuthService _authService;
        private readonly IUserInfoProvider _userInfo;

        public AuthController(IDistvisorAuthService authService, IUserInfoProvider userInfo)
        {
            _authService = authService;
            _userInfo = userInfo;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok($"Hello {_userInfo.UserName}");
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResult), 200)]
        [ProducesResponseType(typeof(AuthResult), 401)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login(LoginRequestDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginResult = await _authService.LoginAsync(login.Username, login.Password);

            if (!loginResult.IsAuthenticated)
            {
                return Unauthorized(loginResult);
            }

            return Ok(loginResult);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (_userInfo.IsAuthenticated)
            {
                await _authService.LogoutAsync(_userInfo.UserId);
            }
            return Ok();
        }
    }

    public class LoginRequestDto
    {
        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }
}
