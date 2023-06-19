using Distvisor.App.Features.HomeBox.Entities;
using Distvisor.App.Features.HomeBox.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Distvisor.Infrastructure.Persistence.App.Configurations
{
    public class DeviceEntityConfiguration : IEntityTypeConfiguration<DeviceEntity>
    {
        public void Configure(EntityTypeBuilder<DeviceEntity> entityType)
        {
            entityType.HasKey(o => o.Id);

            entityType.Property(e => e.Type)
                .HasConversion<string>();
        }
    }
}
