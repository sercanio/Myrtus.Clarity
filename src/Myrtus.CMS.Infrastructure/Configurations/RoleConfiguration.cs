using Myrtus.CMS.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Myrtus.CMS.Infrastructure.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(role => role.Id);

        builder.HasMany(role => role.Permissions)
               .WithMany()
               .UsingEntity<RolePermission>();

        // Seed predefined roles
        builder.HasData(Role.Registered, Role.Admin);
    }
}
