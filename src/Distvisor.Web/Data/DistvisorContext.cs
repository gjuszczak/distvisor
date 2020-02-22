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
        public DbSet<SecretsVaultEntity> SecretsVault { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<OAuthTokenEntity>()
                .Property(e => e.Issuer)
                .HasConversion(
                    v => v.ToString(),
                    v => (OAuthTokenIssuer)Enum.Parse(typeof(OAuthTokenIssuer), v));

            modelBuilder
                .Entity<OAuthTokenEntity>()
                .Property(e => e.UtcIssueDate)
                .HasConversion(
                    v => v,
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder
                .Entity<SecretsVaultEntity>()
                .Property(e => e.Key)
                .HasConversion(
                    v => v.ToString(),
                    v => (SecretKey)Enum.Parse(typeof(SecretKey), v));
        }
    }
}
