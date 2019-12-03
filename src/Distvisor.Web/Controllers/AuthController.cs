using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _users;

        public AuthController(IAuthService users)
        {
            _users = users;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok($"Hello {User.Identity.Name}");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _users.AnyAsync())
            {
                await _users.CreateUserAsync(login.Username, login.Password);
            }

            var loginResult = await _users.LoginAsync(login.Username, login.Password);

            if (!loginResult.IsAuthenticated)
                return Unauthorized(loginResult.Message);

            return Ok(new LoginSuccessResponseDto
            {
                Username = loginResult.Username,
                SessionId = loginResult.SessionId,
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _users.LogoutAsync(User.Identity.Name);
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

    public class LoginSuccessResponseDto
    {
        public string Username { get; set; }
        public string SessionId { get; set; }
    }
}
