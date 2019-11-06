using Distvisor.Web.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Distvisor.Web.Data
{
    public class DistvisorContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        { 
            options.UseSqlite(@"Data Source=Data\dev_distvisor.db");
        }
    }
}
