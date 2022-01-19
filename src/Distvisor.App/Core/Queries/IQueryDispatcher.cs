using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
}
