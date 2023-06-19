using System.Threading.Tasks;

namespace Distvisor.App.Features.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }

        Task<string> GetAccessTokenAsync();
    }
}
