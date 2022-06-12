using Distvisor.App.Common.Interfaces;
using Distvisor.App.HomeBox.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Persistence.App
{
    // Add-Migration Initial -c AppDbContext -o Persistence/App/Migrations
    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly IAuditDataEnricher _auditDataEnricher;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IAuditDataEnricher auditDataEnricher)
            : base(options) 
        {
            _auditDataEnricher = auditDataEnricher;
        }

        public DbSet<DeviceEntity> HomeboxDevices { get; set; }
        public DbSet<GatewaySessionEntity> HomeboxGatewaySessions { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            _auditDataEnricher.Enrich(this);
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("app");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), x => x.Namespace.Contains("App"));
            base.OnModelCreating(builder);
        }
    }
}
