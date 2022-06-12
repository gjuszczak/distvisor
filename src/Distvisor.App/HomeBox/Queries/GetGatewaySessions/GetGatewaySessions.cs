using Distvisor.App.Common.Interfaces;
using Distvisor.App.Common.Models;
using Distvisor.App.Core.Queries;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Queries.GetGatewaySessions
{
    public class GetGatewaySessions : PaginatedQuery, IQuery<PaginatedList<GatewaySessionDto>>
    {
    }

    public class GetGatewaySessionsHandler : IQueryHandler<GetGatewaySessions, PaginatedList<GatewaySessionDto>>
    {
        private readonly IAppDbContext _context;

        public GetGatewaySessionsHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<GatewaySessionDto>> Handle(GetGatewaySessions request, CancellationToken cancellationToken)
        {
            var gatewaySessions = await _context.HomeboxGatewaySessions
                .Select(entity => GatewaySessionDto.FromEntity(entity))
                .ToPaginatedListAsync(request.FirstOffset, request.PageSize, cancellationToken);
            return gatewaySessions;
        }
    }
}
