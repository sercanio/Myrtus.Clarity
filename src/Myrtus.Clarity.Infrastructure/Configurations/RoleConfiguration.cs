using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Users;
using System.Collections.Generic;

namespace Myrtus.Clarity.Infrastructure.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");
            builder.HasKey(r => r.Id);

            // 1) We define a stable GUID for the admin role
            var adminRoleId = Guid.Parse("4b606d86-3537-475a-aa20-26aadd8f5cfd");

            // 2) Seed the Admin role itself (the domain class typically has a static Admin,
            //    but let's be explicit here).
            builder.HasData(
            new
            {
                Id = adminRoleId,
                Name = "Admin",
                IsDefault = false,
                CreatedBy = "System",
                CreatedOnUtc = DateTime.UtcNow,
            });

            // 3) If you haven't already configured Roles<->Permissions in a separate config,
            //    do it here (assuming direct many-to-many approach).
            builder
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "role_permission",
                    // Table for the Role-Permission many-to-many
                    rp => rp
                        .HasOne<Permission>()
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .HasConstraintName("FK_role_permission_permissions_PermissionId"),
                    rp => rp
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_role_permission_roles_RoleId"),
                    rp =>
                    {
                        rp.HasKey("RoleId", "PermissionId");

                        // Add AdminRole <-> All Permissions
                        // We can reference the static Permission objects
                        rp.HasData(
                            new { RoleId = adminRoleId, PermissionId = Permission.UsersRead.Id },
                            new { RoleId = adminRoleId, PermissionId = Permission.UsersCreate.Id },
                            new { RoleId = adminRoleId, PermissionId = Permission.UsersUpdate.Id },
                            new { RoleId = adminRoleId, PermissionId = Permission.UsersDelete.Id },
                            new { RoleId = adminRoleId, PermissionId = Permission.RolesRead.Id },
                            new { RoleId = adminRoleId, PermissionId = Permission.RolesCreate.Id },
                            new { RoleId = adminRoleId, PermissionId = Permission.RolesUpdate.Id },
                            new { RoleId = adminRoleId, PermissionId = Permission.RolesDelete.Id },
                            new { RoleId = adminRoleId, PermissionId = Permission.PermissionsRead.Id },
                            new { RoleId = adminRoleId, PermissionId = Permission.AuditLogsRead.Id },
                            new { RoleId = adminRoleId, PermissionId = Permission.NotificationsRead.Id },
                            new { RoleId = adminRoleId, PermissionId = Permission.NotificationsUpdate.Id }
                        );
                    }
                );

            // 4) If you also want to add the admin User -> Admin Role link, do it similarly:
            //    The same direct many-to-many for Roles <-> Users
            builder
                .HasMany(r => r.Users)
                .WithMany(u => u.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "role_user",
                    ru => ru
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_role_user_users_UserId"),
                    ru => ru
                        .HasOne<Role>()
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_role_user_roles_RoleId"),
                    ru =>
                    {
                        ru.HasKey("RoleId", "UserId");

                        // Must match the stable AdminUserId used in UserConfiguration
                        ru.HasData(
                            new
                            {
                                RoleId = adminRoleId,
                                UserId = Guid.Parse("55c7f429-0916-4d84-8b76-d45185d89aa7")
                            }
                        );
                    }
                );
        }
    }
}
