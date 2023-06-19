using Distvisor.App.Core.Aggregates;
using Distvisor.App.Core.Commands;
using Distvisor.App.Features.Redirections.Aggregates;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.Redirections.Commands.EditRedirection
{
    public class EditRedirection : RedirectionBaseCommand
    {
        public Guid RedirectionId { get; set; }
    }

    public class EditRedirectionHandler : ICommandHandler<EditRedirection>
    {
        private readonly IAggregateContext _aggregateContext;

        public EditRedirectionHandler(IAggregateContext aggregateContext)
        {
            _aggregateContext = aggregateContext;
        }

        public async Task<Guid> Handle(EditRedirection command, CancellationToken cancellationToken)
        {
            var redirection = await _aggregateContext.GetAsync<Redirection>(command.RedirectionId, cancellationToken);
            var url = new Uri(command.Url, UriKind.Absolute);
            redirection.Edit(command.Name, url);
            await _aggregateContext.CommitAsync(cancellationToken);
            return redirection.AggregateId;
        }
    }
}
