using Distvisor.App.Core.Enums;
using Distvisor.App.HomeBox.Entities;
using Distvisor.App.HomeBox.Enums;
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
                .HasConversion(
                    v => v.Name,
                    v => Enumeration.FromName<DeviceType>(v));
        }
    }
}
