using System;
using System.Threading.Tasks;

namespace Distvisor.App.Features.HomeBox.Services.Gateway
{
    public interface IGatewayAuthenticationPolicy
    {
        Task<T> ExecuteWithTokenAsync<T>(Func<string, Task<T>> action);
    }
}
