using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<Guid> Handle(TCommand command, CancellationToken cancellationToken);
    }
}
