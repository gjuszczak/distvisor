using Distvisor.App.Common.Interfaces;
using Distvisor.App.Core.Events;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Persistence.Events
{
    // Add-Migration Initial -c EventsDbContext -o Persistence/Events/Migrations
    public class EventsDbContext : DbContext, IEventsDbContext
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
