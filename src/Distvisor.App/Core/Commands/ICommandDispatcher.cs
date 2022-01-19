using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Commands
{
    public interface ICommandDispatcher
    {
        Task<Guid> DispatchAsync(ICommand command, CancellationToken cancellationToken = default);
    }
}
