using Distvisor.App.Core.Dispatchers;
using System;

namespace Distvisor.App.Core.Commands
{
    public interface ICommandHandler<in TCommand> : IHandler<TCommand, Guid> 
        where TCommand : ICommand
    {
    }
}
