using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserInfoProvider _userInfo;

        public AccountController(IUserInfoProvider accountInfo)
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
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
