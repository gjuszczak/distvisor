using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Commands;
using Distvisor.App.Features.Redirections.Aggregates;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.Redirections.Commands.CreateRedirection
{
    public class CreateRedirection : RedirectionBaseCommand
    {
    }

    public class CreateRedirectionHandler : ICommandHandler<CreateRedirection>
    {
        private readonly IAggregateContext _aggregateContext;

        public CreateRedirectionHandler(IAggregateContext aggregateContext)
        {
            _aggregateContext = aggregateContext;
        }

        public async Task<Guid> Handle(CreateRedirection command, CancellationToken cancellationToken)
        {
            var url = new Uri(command.Url, UriKind.Absolute);
            var redirection = new Redirection(command.Id, command.Name, url);
            _aggregateContext.Add(redirection);
            await _aggregateContext.CommitAsync(cancellationToken);
            return redirection.AggregateId;
        }
    }
}
