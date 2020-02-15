using Distvisor.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Distvisor.Web.Data
{
    public class DistvisorContext : DbContext
    {
        public DistvisorContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<SecretsVaultEntity> SecretsVault { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<SecretsVaultEntity>()
                .Property(e => e.Key)
                .HasConversion(
                    v => v.ToString(),
                    v => (SecretKey)Enum.Parse(typeof(SecretKey), v));
        }
    }
}
