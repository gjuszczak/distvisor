using Distvisor.Web.Data.Reads.Entities;
using Microsoft.EntityFrameworkCore;

namespace Distvisor.Web.Data.Reads.Core
{
    // docker run --name postgresdb -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres:15-alpine
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

            modelBuilder.Entity<HomeBoxTriggerSourceEntity>()
                .HasOne(e => e.Trigger)
                .WithMany(e => e.Sources)
                .HasForeignKey(e => e.TriggerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HomeBoxTriggerTargetEntity>()
                .HasOne(e => e.Trigger)
                .WithMany(e => e.Targets)
                .HasForeignKey(e => e.TriggerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HomeBoxTriggerActionEntity>()
                .HasOne(e => e.Trigger)
                .WithMany(e => e.Actions)
                .HasForeignKey(e => e.TriggerId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<SecretsVaultEntity> SecretsVault { get; set; }
        public DbSet<RedirectionEntity> Redirections { get; set; }
        public DbSet<FinancialAccountEntity> FinancialAccounts { get; set; }
        public DbSet<FinancialAccountTransactionEntity> FinancialAccountTransactions { get; set; }
        public DbSet<HomeBoxDeviceEntity> HomeboxDevices { get; set; }
        public DbSet<HomeBoxTriggerEntity> HomeboxTriggers { get; set; }
        public DbSet<HomeBoxTriggerSourceEntity> HomeboxTriggerSources { get; set; }
        public DbSet<HomeBoxTriggerTargetEntity> HomeboxTriggerTargets { get; set; }
        public DbSet<HomeBoxTriggerActionEntity> HomeboxTriggerActions { get; set; }
    }
}
