using Distvisor.App.Common.Interfaces;
using Distvisor.App.HomeBox.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Persistence.App
{
    // Add-Migration Initial -c AppDbContext -o Persistence/App/Migrations
    // docker run --name postgresdb -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres:15-alpine
    // docker run --name pgadmin4 -p 8080:80 -e 'PGADMIN_DEFAULT_EMAIL=admin@admin.com' -e 'PGADMIN_DEFAULT_PASSWORD=admin' -d dpage/pgadmin4
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

        public async Task<IDbContextTransaction> DeleteAllData(CancellationToken cancellationToken)
        {
            var tables = Model.GetEntityTypes().Select(x => $"{x.GetSchema()}.\"{x.GetTableName()}\"");
            var transaction = await Database.BeginTransactionAsync(cancellationToken);
            foreach (var table in tables)
            {
                await Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {table} CASCADE;", cancellationToken: cancellationToken);
            }
            return transaction;
        }
    }
}
