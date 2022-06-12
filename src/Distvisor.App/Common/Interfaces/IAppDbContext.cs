using Distvisor.App.HomeBox.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<DeviceEntity> HomeboxDevices { get; set; }
        DbSet<GatewaySessionEntity> HomeboxGatewaySessions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
