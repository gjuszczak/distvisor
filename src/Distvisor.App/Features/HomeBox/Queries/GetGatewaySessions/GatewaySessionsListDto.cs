using Distvisor.App.Features.Common.Models;
using System.Collections.Generic;

namespace Distvisor.App.Features.HomeBox.Queries.GetGatewaySessions
{
    public class GatewaySessionsListDto : PaginatedList<GatewaySessionDto>
    {
        public GatewaySessionsListDto(List<GatewaySessionDto> items, int totalRecords, int first, int rows)
            : base(items, totalRecords, first, rows) { }
    }
}
