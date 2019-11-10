using Distvisor.Web.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Distvisor.Web.Data
{
    public class DistvisorContext : DbContext
    {
        public DistvisorContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
