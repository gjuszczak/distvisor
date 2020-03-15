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
        [ProducesResponseType(typeof(AuthUser), 200)]
        [ProducesResponseType(typeof(string), 401)]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var authResult = await _authService.LoginAsync(dto.Username, dto.Password);

            if (!authResult.IsAuthenticated)
            {
                return Unauthorized(authResult.Message);
            }

            return Ok(new AuthUser
            {
                Username = authResult.Username,
                AccessToken = authResult.Token.AccessToken,
                RefreshToken = authResult.Token.RefreshToken,
            });
        }

        [HttpPost("refreshtoken")]
        [ProducesResponseType(typeof(AuthUser), 200)]
        [ProducesResponseType(typeof(string), 401)]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto dto)
        {
            var authResult = await _authService.RefreshAccessTokenAsync(dto.RefreshToken);

            if (!authResult.IsAuthenticated)
            {
                return Unauthorized(authResult.Message);
            }

            return Ok(new AuthUser
            {
                Username = authResult.Username,
                AccessToken = authResult.Token.AccessToken,
                RefreshToken = authResult.Token.RefreshToken,
            });
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

    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }

    public class AuthUser
    {
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
