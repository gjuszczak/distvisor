using System;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Services.Gateway
{
    public interface IGatewaySessionManager
    {
        public Task<GatewayActiveSession> GetActiveSessionAsync();
        public Task RefreshSessionAsync(Guid sessionId);
    }
}
