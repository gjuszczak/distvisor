using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Common.Interfaces
{
    public interface IEventsReplayService
    {
        Task ReplayAsync(CancellationToken cancellationToken);
    }
}