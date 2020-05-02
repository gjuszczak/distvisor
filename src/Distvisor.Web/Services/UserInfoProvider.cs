using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IUserInfoProvider
    {
        bool IsAuthenticated { get; }
        string UserName { get; }
        Guid UserId { get; }

        Task<string> GetAccessTokenAsync();
    }

    public class UserInfoProvider : IUserInfoProvider
    {
        private readonly HttpContext _httpContext;

        public UserInfoProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public bool IsAuthenticated => _httpContext.User.Identity.IsAuthenticated;

        public string UserName => _httpContext.User.FindFirstValue("preferred_username");

        public Guid UserId => Guid.Parse(_httpContext.User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier"));

        public async Task<string> GetAccessTokenAsync() => await _httpContext.GetTokenAsync("access_token");
    }
}
