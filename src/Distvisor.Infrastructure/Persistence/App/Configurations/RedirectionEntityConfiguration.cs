using Distvisor.App.Features.Redirections.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Distvisor.Infrastructure.Persistence.App.Configurations
{
    public class RedirectionEntityConfiguration : IEntityTypeConfiguration<RedirectionEntity>
    {
        public void Configure(EntityTypeBuilder<RedirectionEntity> entityType)
        {
            entityType.HasKey(o => o.Id);
            entityType.HasIndex(e => e.Name).IsUnique();
        }
    }
}
