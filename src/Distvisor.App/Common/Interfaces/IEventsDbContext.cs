using Distvisor.App.Core.Events;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Common.Interfaces
{
    public interface IEventsDbContext
    {
        DbSet<EventEntity> Events { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
