using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IDockerService
    {
        Task<IEnumerable<string>> GetAllContainersAsync();
    }
}