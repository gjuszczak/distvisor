using Distvisor.App.Core.Enums;
using Distvisor.App.HomeBox.Entities;
using Distvisor.App.HomeBox.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Distvisor.Infrastructure.Persistence.App.Configurations
{
    public class GatewaySessionEntityConfiguration : IEntityTypeConfiguration<GatewaySessionEntity>
    {
        public void Configure(EntityTypeBuilder<GatewaySessionEntity> entityType)
        {
            entityType.HasKey(o => o.Id);

            entityType.OwnsOne(o => o.Token, navigationBuilder =>
            {
                navigationBuilder.Property(t => t.AccessToken)
                    .HasColumnName("AccessToken");
                navigationBuilder.Property(t => t.RefreshToken)
                    .HasColumnName("RefreshToken");
                navigationBuilder.Property(t => t.GeneratedAt)
                    .HasColumnName("TokenGeneratedAt");
            });

            entityType.Property(e => e.Status)
                .HasConversion(
                    v => v.Name,
                    v => Enumeration.FromName<GatewaySessionStatus>(v));
        }
    }
}
