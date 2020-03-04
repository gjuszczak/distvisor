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
        public DbSet<OAuthTokenEntity> OAuthTokens { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<SecretsVaultEntity> SecretsVault { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<OAuthTokenEntity>()
                .Property(e => e.Issuer)
                .HasEnumConversion();

            modelBuilder
                .Entity<OAuthTokenEntity>()
                .Property(e => e.UtcIssueDate)
                .HasUtcDateTimeConversion();

            modelBuilder
                .Entity<SecretsVaultEntity>()
                .Property(e => e.Key)
                .HasEnumConversion();
        }
    }
}
