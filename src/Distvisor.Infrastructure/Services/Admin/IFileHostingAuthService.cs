using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Services.Admin
{
    public interface IFileHostingAuthService
    {
        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken);
    }
}