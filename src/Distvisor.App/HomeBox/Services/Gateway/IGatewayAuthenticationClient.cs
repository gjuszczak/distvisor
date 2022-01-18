using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Services.Gateway
{
    public interface IGatewayAuthenticationClient
    {
        Task<GatewayAuthenticationResponse> LoginAsync(string username, string password);
        Task<GatewayAuthenticationResponse> RefreshSessionAsync(string refreshToken);
    }
}
