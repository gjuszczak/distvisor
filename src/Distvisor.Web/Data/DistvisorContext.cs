using Distvisor.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Distvisor.Web.Data
{
    public class DistvisorContext : DbContext
    {
        public DistvisorContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
    }
}
