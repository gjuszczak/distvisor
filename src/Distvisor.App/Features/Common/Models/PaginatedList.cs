using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.Common.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int TotalRecords { get; }
        public int First { get; }
        public int Rows { get; }

        public PaginatedList(List<T> items, int totalRecords, int first, int rows)
        {
            Items = items;
            TotalRecords = totalRecords;
            First = first;
            Rows = rows;
        }

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source,
            int first,
            int rows,
            CancellationToken cancellationToken = default)
        {
            var totalRecords = await source.CountAsync(cancellationToken);
            var items = await source.Skip(first).Take(rows).ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, totalRecords, first, rows);
        }
    }
}
