using Distvisor.App.Core.Queries;
using Distvisor.App.Features.Common.Interfaces;
using Distvisor.App.Features.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.HomeBox.Queries.GetGatewaySessions
{
    public class GetGatewaySessions : PaginatedQuery, IQuery<GatewaySessionsListDto>
    {
    }

    public class GetGatewaySessionsHandler : IQueryHandler<GetGatewaySessions, GatewaySessionsListDto>
    {
        private readonly IAppDbContext _context;

        public GetGatewaySessionsHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<GatewaySessionsListDto> Handle(GetGatewaySessions request, CancellationToken cancellationToken)
        {
            var gatewaySessions = await _context.HomeboxGatewaySessions
                .Select(entity => GatewaySessionDto.FromEntity(entity))
                .ToPaginatedListAsync(request.First, request.Rows, cancellationToken);

            return new GatewaySessionsListDto(
                gatewaySessions.Items,
                gatewaySessions.TotalRecords,
                gatewaySessions.First,
                gatewaySessions.Rows);
        }
    }
}
