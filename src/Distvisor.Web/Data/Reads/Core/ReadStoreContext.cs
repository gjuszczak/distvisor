using Distvisor.Web.Data.Entities;
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

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<OAuthTokenEntity> OAuthTokens { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<SecretsVaultEntity> SecretsVault { get; set; }
        public DbSet<RedirectionEntity> Redirections { get; set; }
    }
}
