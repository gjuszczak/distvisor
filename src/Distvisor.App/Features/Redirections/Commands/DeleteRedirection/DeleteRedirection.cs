using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Commands;
using Distvisor.App.Features.Redirections.Aggregates;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.Redirections.Commands.DeleteRedirection
{
    public class DeleteRedirection : Command
    {
        public Guid RedirectionId { get; set; }
    }

    public class DeleteRedirectionHandler : ICommandHandler<DeleteRedirection>
    {
        private readonly IAggregateContext _aggregateContext;

        public DeleteRedirectionHandler(IAggregateContext aggregateContext)
        {
            _aggregateContext = aggregateContext;
        }

        public async Task<Guid> Handle(DeleteRedirection command, CancellationToken cancellationToken)
        {
            var redirection = await _aggregateContext.GetAsync<Redirection>(command.RedirectionId, cancellationToken);
            redirection.Delete();
            await _aggregateContext.CommitAsync(cancellationToken);
            return redirection.AggregateId;
        }
    }
}
