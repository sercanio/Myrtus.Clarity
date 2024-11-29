using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Myrtus.Clarity.Domain.Roles;

namespace Myrtus.Clarity.Infrastructure.Configurations
{
    internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("permissions");
            builder.HasKey(p => p.Id);
        }
    }
}
