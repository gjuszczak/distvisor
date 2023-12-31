using Distvisor.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Distvisor.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserInfoProvider _userInfo;

        public UserController(IUserInfoProvider accountInfo)
        {
            _userInfo = accountInfo;
        }

        [HttpGet]
        public UserInfoDto GetUserInfo()
        {
            return new UserInfoDto
            {
                UserId = _userInfo.UserId,
                Username = _userInfo.Username,
                Role = _userInfo.Role
            };
        }
    }

    public class UserInfoDto
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
