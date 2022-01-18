using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Queries
{
    public interface IQueryBus
    {
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default(CancellationToken));
    }
}
