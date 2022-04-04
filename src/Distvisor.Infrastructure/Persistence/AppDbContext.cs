using Distvisor.App.Common;
using Distvisor.App.Core.Entities;
using Distvisor.App.Core.Events;
using Distvisor.App.Core.Services;
using Distvisor.App.HomeBox.Entities;
using Distvisor.App.HomeBox.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure
{
    // Add-Migration Initial -c AppDbContext -o Persistence/Migrations
    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public AppDbContext(
            DbContextOptions<AppDbContext> options, 
            ICorrelationIdProvider correlationIdProvider)
            : base(options) 
        {
            _correlationIdProvider = correlationIdProvider;
        }

        public DbSet<EventEntity> Events { get; set; }

        public DbSet<DeviceEntity> HomeboxDevices { get; set; }
        public DbSet<DeviceType> HomeboxDeviceTypes { get; set; }
        public DbSet<GatewaySessionEntity> HomeboxGatewaySessions { get; set; }
        public DbSet<GatewaySessionStatus> HomeboxGatewaySessionStatuses { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _correlationIdProvider.GetCorrelationId().ToString();
                        entry.Entity.Created = DateTimeOffset.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _correlationIdProvider.GetCorrelationId().ToString();
                        entry.Entity.LastModified = DateTimeOffset.Now;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
