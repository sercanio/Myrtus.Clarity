using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Infrastructure.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(permission => permission.Id);

        // Users
        builder.HasData(Permission.UsersRead);
        builder.HasData(Permission.UsersCreate);
        builder.HasData(Permission.UsersUpdate);
        builder.HasData(Permission.UsersDelete);

        // Roles
        builder.HasData(Permission.RolesRead);
        builder.HasData(Permission.RolesCreate);
        builder.HasData(Permission.RolesUpdate);
        builder.HasData(Permission.RolesDelete);

        // Permissions
        builder.HasData(Permission.PermissionsRead);
    }
}
