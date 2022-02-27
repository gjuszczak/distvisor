using Distvisor.App.Core.Commands;
using Distvisor.App.Core.Queries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Dispatchers
{
    public interface IDispatcher
    {
        Task<Guid> DispatchAsync(ICommand command, CancellationToken cancellationToken = default);
        Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
}