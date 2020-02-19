using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Distvisor.Web.Services
{
    public interface IUserInfoProvider
    {
        Guid UserId { get; }
        string UserName { get; }
    }

    public class UserInfoProvider : IUserInfoProvider
    {
        private readonly ClaimsPrincipal _user;

        public UserInfoProvider(IHttpContextAccessor httpContextAccessor)
        {
            _user = httpContextAccessor.HttpContext.User;
        }

        public string UserName => _user.Identity.Name;

        public Guid UserId => Guid.Parse(_user.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}
