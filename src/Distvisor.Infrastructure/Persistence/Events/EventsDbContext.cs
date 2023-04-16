using Distvisor.App.Core.Events;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Persistence.Events
{
    // Add-Migration Initial -c EventsDbContext -o Persistence/Events/Migrations
    // docker run --name postgresdb -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres:15-alpine
    // docker run --name pgadmin4 -p 8080:80 -e 'PGADMIN_DEFAULT_EMAIL=admin@admin.com' -e 'PGADMIN_DEFAULT_PASSWORD=admin' -d dpage/pgadmin4
    public class EventsDbContext : DbContext
    {
        private readonly IAuditDataEnricher _auditDataEnricher;

        public EventsDbContext(
            DbContextOptions<EventsDbContext> options,
            IAuditDataEnricher auditDataEnricher)
            : base(options)
        {
            _auditDataEnricher = auditDataEnricher;
        }

        public DbSet<EventEntity> Events { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            _auditDataEnricher.Enrich(this);
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("events");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), x => x.Namespace.Contains("Events"));
            base.OnModelCreating(builder);
        }
    }
}
