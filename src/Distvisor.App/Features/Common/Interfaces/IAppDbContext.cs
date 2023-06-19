using Distvisor.App.Features.HomeBox.Entities;
using Distvisor.App.Features.Redirections.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<DeviceEntity> HomeboxDevices { get; }
        DbSet<GatewaySessionEntity> HomeboxGatewaySessions { get; }
        DbSet<RedirectionEntity> Redirections { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<IDbContextTransaction> DeleteAllData(CancellationToken cancellationToken);
    }
}
