using MediatR;
using System;

namespace Distvisor.App.Core.Commands
{
    public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest, Guid> 
        where TRequest : ICommand
    {
    }
}
