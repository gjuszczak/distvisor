using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Commands
{
    public interface ICommandBus
    {
        Task<Guid> ExecuteAsync(ICommand command, CancellationToken cancellationToken = default(CancellationToken));
    }
}
