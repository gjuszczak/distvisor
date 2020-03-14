﻿using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Distvisor.Web.Services
{
    public interface IUserInfoProvider
    {
        bool IsAuthenticated { get; }
        string UserName { get; }
        Guid UserId { get; }
    }

    public class UserInfoProvider : IUserInfoProvider
    {
        private readonly ClaimsPrincipal _user;

        public UserInfoProvider(IHttpContextAccessor httpContextAccessor)
        {
            _user = httpContextAccessor.HttpContext.User;
        }

        public bool IsAuthenticated => _user.Identity.IsAuthenticated;

        public string UserName => _user.Identity.Name;

        public Guid UserId => Guid.Parse(_user.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}
