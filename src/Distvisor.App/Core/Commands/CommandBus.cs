using Distvisor.App.Core.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly IMediator _mediator;
        public CommandBus(ICorrelationIdProvider correlationIdProvider, IMediator mediator)
        {
            _correlationIdProvider = correlationIdProvider;
            _mediator = mediator;
        }

        public async Task<Guid> ExecuteAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            _correlationIdProvider.SetCorrelationId(command.CorrelationId);
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
