using Distvisor.App.HomeBox.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Distvisor.Infrastructure.Persistence.Configurations
{
    public class GatewaySessionStatusConfiguration : IEntityTypeConfiguration<GatewaySessionStatus>
    {
        public void Configure(EntityTypeBuilder<GatewaySessionStatus> entityType)
        {
            entityType.HasKey(o => o.Id);

            entityType.Property(o => o.Id)
                .ValueGeneratedNever()
                .IsRequired();

            entityType.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
