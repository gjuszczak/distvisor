using Distvisor.App.Features.Common.Models;
using System.Collections.Generic;

namespace Distvisor.App.Features.Redirections.Queries.GetRedirections
{
    public class RedirectionsListDto : PaginatedList<RedirectionDto>
    {
        public RedirectionsListDto(List<RedirectionDto> items, int totalRecords, int first, int rows)
            : base(items, totalRecords, first, rows) { }
    }
}
