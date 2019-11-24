using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IDockerService
    {
        Task UpdateImageAsync(string tag);
    }
}