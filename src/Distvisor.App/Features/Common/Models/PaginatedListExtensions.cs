using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.Common.Models
{
    public static class PaginatedListExtensions
    {
        public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(
            this IQueryable<TDestination> queryable,
            int firstOffset,
            int pageSize,
            CancellationToken cancellationToken = default)
            => PaginatedList<TDestination>.CreateAsync(queryable, firstOffset, pageSize, cancellationToken);
    }
}
