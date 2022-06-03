using Distvisor.App.Common;
using Distvisor.App.Core.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Queries.GetGatewaySessions
{
    public class GetGatewaySessions : IQuery<IEnumerable<GatewaySessionDto>>
    {
    }

    public class GetGatewaySessionsHandler : IQueryHandler<GetGatewaySessions, IEnumerable<GatewaySessionDto>>
    {
        private readonly IAppDbContext _context;

        public GetGatewaySessionsHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GatewaySessionDto>> Handle(GetGatewaySessions request, CancellationToken cancellationToken)
        {
            var gatewaySessions = await _context.HomeboxGatewaySessions.ToListAsync(cancellationToken);
            var gatewaySessionDtos = gatewaySessions.Select(GatewaySessionDto.FromEntity).ToArray();
            return gatewaySessionDtos;
        }
    }
}
