using Distvisor.Web.Data.Reads.Entities;
using Microsoft.EntityFrameworkCore;

namespace Distvisor.Web.Data.Reads.Core
{
    // docker run --name postgresdb -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres:12.3-alpine
    // docker run --name adminer -p 8080:8080 -d adminer
    // Add-Migration InitialCreate -c ReadStoreContext -o Data/Reads/Migrations
    public class ReadStoreContext : DbContext
    {
        public ReadStoreContext()
        {
        }

        public ReadStoreContext(DbContextOptions<ReadStoreContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProcessedEmailEntity>()
                .HasIndex(e => e.UniqueKey)
                .IsUnique();
        }

        public DbSet<SecretsVaultEntity> SecretsVault { get; set; }
        public DbSet<RedirectionEntity> Redirections { get; set; }
        public DbSet<ProcessedEmailEntity> ProcessedEmails { get; set; }
        public DbSet<FinancialAccountEntity> FinancialAccounts { get; set; }
        public DbSet<FinancialAccountPaycardEntity> FinancialAccountPaycards { get; set; }
    }
}
