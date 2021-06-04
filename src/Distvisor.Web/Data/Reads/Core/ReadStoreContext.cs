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
            modelBuilder.Entity<FinancialAccountEntity>()
                .HasIndex(e => e.Number)
                .IsUnique();

            modelBuilder.Entity<FinancialAccountEntity>()
                .Property(e => e.Type)
                .HasConversion<string>();

            modelBuilder.Entity<FinancialAccountTransactionEntity>()
                .HasOne(e => e.Account)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FinancialAccountTransactionEntity>()
                .Property(e => e.Source)
                .HasConversion<string>();

            modelBuilder.Entity<FinancialAccountTransactionEntity>()
                .HasIndex(e => new { e.AccountId, e.SeqNo })
                .IsUnique();

            modelBuilder.Entity<FinancialAccountTransactionEntity>()
                .HasIndex(e => e.TransactionHash)
                .IsUnique();

            modelBuilder.Entity<FinancialAccountTransactionEntity>()
                .Property(e => e.TransactionDate)
                .HasColumnType("DATE");

            modelBuilder.Entity<FinancialAccountTransactionEntity>()
                .Property(e => e.PostingDate)
                .HasColumnType("DATE");
        }

        public DbSet<SecretsVaultEntity> SecretsVault { get; set; }
        public DbSet<RedirectionEntity> Redirections { get; set; }
        public DbSet<FinancialAccountEntity> FinancialAccounts { get; set; }
        public DbSet<FinancialAccountTransactionEntity> FinancialAccountTransactions { get; set; }
    }
}
