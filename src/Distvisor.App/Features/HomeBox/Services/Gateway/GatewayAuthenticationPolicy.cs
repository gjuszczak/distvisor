using System;
using System.Threading.Tasks;

namespace Distvisor.App.Features.HomeBox.Services.Gateway
{
    public class GatewayAuthenticationPolicy : IGatewayAuthenticationPolicy
    {
        private readonly IGatewaySessionManager _sessionManager;

        public GatewayAuthenticationPolicy(IGatewaySessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task<T> ExecuteWithTokenAsync<T>(Func<string, Task<T>> action)
        {
            var activeSession = await GetActiveSessionAsync();
            try
            {
                return await ExecuteWithTokenAsync(activeSession.AccessToken, action);
            }
            catch (InvalidGatewayTokenException)
            {
                await _sessionManager.RefreshSessionAsync(activeSession.SessionId);
                activeSession = await GetActiveSessionAsync();
                return await ExecuteWithTokenAsync(activeSession.AccessToken, action);
            }
        }

        private async Task<T> ExecuteWithTokenAsync<T>(string accessToken, Func<string, Task<T>> action)
        {
            if (accessToken == null)
            {
                throw new InvalidGatewayTokenException();
            }
            return await action(accessToken);
        }

        private async Task<GatewayActiveSession> GetActiveSessionAsync()
        {
            var activeSession = await _sessionManager.GetActiveSessionAsync();
            if (activeSession?.AccessToken == null)
            {
                throw new InvalidOperationException("Active gateway session is not available. Please login to gateway api.");
            }
            return activeSession;
        }
    }
}
