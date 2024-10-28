using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Infrastructure.Configurations;

internal sealed class RoleUserConfiguration : IEntityTypeConfiguration<RoleUser>
{
    public void Configure(EntityTypeBuilder<RoleUser> builder)
    {
        builder.ToTable("role_users");
        builder.HasKey(userRole => new { userRole.RoleId, userRole.UserId});

        builder.HasOne(userRole => userRole.Role)
           .WithMany(role => role.RoleUsers)
           .HasForeignKey(userRole => userRole.RoleId);

        builder.HasOne(userRole => userRole.User)
            .WithMany(user => user.UserRoles)
            .HasForeignKey(userRole => userRole.UserId);


        builder.HasData(new RoleUser
        {
            RoleId = Role.Registered.Id,
            UserId = UserConfiguration.AdminId
        });

        // Seed RoleUser relationship for Admin user and Admin role
        builder.HasData(new RoleUser
        {
            RoleId = Role.Admin.Id,
            UserId = UserConfiguration.AdminId
        });

    }
}
