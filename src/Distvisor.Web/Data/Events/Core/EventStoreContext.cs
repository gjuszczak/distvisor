using Distvisor.Web.Data.Events.Entities;
using Microsoft.EntityFrameworkCore;

namespace Distvisor.Web.Data.Events.Core
{
    // docker run --name postgresdb -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres:12.3-alpine
    // docker run --name adminer -p 8080:8080 -d adminer
    // Add-Migration InitialCreate -c EventStoreContext -o Data/Events/Migrations
    public class EventStoreContext : DbContext
    {
        public EventStoreContext()
        {
        }

        public EventStoreContext(DbContextOptions<EventStoreContext> options) : base(options)
        {
        }

        public DbSet<EventEntity> Events { get; set; }
    }
}
