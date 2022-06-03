using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Services.Gateway
{
    public interface IGatewaySessionManager
    {
        Task<GatewayActiveSession> GetActiveSessionAsync(CancellationToken cancellationToken = default);
        Task OpenGatewaySessionAsync(Guid sessionId, string username, string password, CancellationToken cancellationToken = default);
        Task RefreshSessionAsync(Guid sessionId, CancellationToken cancellationToken = default);
    }
}
