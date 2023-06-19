using Distvisor.App.Features.Redirections.Queries;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Services.Redirections
{
    public interface ICachedRedirectionsService
    {
        Task<RedirectionDto> GetByNameAsync(string name);
    }
}