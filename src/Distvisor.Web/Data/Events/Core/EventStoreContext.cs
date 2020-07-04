using Microsoft.EntityFrameworkCore;

namespace Distvisor.Web.Data.Events.Core
{
    // docker run --name postgresdb -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres:12.3-alpine
    // Add-Migration InitialCreate -c EventStoreContext -o Data/Events/Migrations
    public class EventStoreContext : DbContext
    {
        public EventStoreContext()
        {
        }

        public EventStoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<EventEntity> Events { get; set; }
    }
}
