using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Common.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int TotalCount { get; }
        public int FirstOffset { get; }
        public int PageSize { get; }

        public PaginatedList(List<T> items, int count, int firstOffset, int pageSize)
        {
            Items = items;
            TotalCount = count;
            FirstOffset = firstOffset;
            PageSize = pageSize;
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int firstOffset, int pageSize, CancellationToken cancellationToken = default)
        {
            var count = await source.CountAsync(cancellationToken);
            var items = await source.Skip(firstOffset).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, count, firstOffset, pageSize);
        }
    }
}
