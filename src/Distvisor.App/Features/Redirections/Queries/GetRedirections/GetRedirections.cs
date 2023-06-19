using Distvisor.App.Core.Queries;
using Distvisor.App.Features.Common.Interfaces;
using Distvisor.App.Features.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.Redirections.Queries.GetRedirections
{
    public class GetRedirections : PaginatedQuery, IQuery<RedirectionsListDto>
    {
    }

    public class GetRedirectionsHandler : IQueryHandler<GetRedirections, RedirectionsListDto>
    {
        private readonly IAppDbContext _context;

        public GetRedirectionsHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<RedirectionsListDto> Handle(GetRedirections request, CancellationToken cancellationToken)
        {
            var redirections = await _context.Redirections
                .Select(entity => RedirectionDto.FromEntity(entity))
                .ToPaginatedListAsync(request.First, request.Rows, cancellationToken);

            return new RedirectionsListDto(redirections.Items, redirections.TotalRecords, redirections.First, redirections.Rows);
        }
    }
}
