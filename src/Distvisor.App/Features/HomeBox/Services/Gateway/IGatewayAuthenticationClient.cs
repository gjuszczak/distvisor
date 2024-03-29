﻿using System.Threading.Tasks;

namespace Distvisor.App.Features.HomeBox.Services.Gateway
{
    public interface IGatewayAuthenticationClient
    {
        Task<GatewayAuthenticationResult> LoginAsync(string username, string password);
        Task<GatewayAuthenticationResult> RefreshSessionAsync(string refreshToken);
    }
}
