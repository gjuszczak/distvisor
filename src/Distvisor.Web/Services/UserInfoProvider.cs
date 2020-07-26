﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IUserInfoProvider
    {
        bool IsAuthenticated { get; }
        string Username { get; }
        string UserId { get; }
        string Role { get; }

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

        public string Username => _httpContext.User.FindFirstValue(ClaimTypes.Name);

        public string UserId => _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public string Role => _httpContext.User.IsInRole("user") ? "user" : "guest";

        public async Task<string> GetAccessTokenAsync() => await _httpContext.GetTokenAsync("access_token");
    }
}
