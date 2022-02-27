using System;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Services.Gateway
{
    public interface IGatewaySessionManager
    {
        Task<GatewayActiveSession> GetActiveSessionAsync();
        Task OpenGatewaySessionAsync(Guid sessionId, string username, string password);
        Task RefreshSessionAsync(Guid sessionId);
    }
}
