using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Core.Queries
{
    public class QueryBus : IQueryBus
    {
        private readonly IMediator _mediator;
        public QueryBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(query, cancellationToken);
        }
    }
}
