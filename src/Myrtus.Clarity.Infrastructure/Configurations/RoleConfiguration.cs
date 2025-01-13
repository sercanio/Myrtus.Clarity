// src/Infrastructure/Configurations/RoleConfiguration.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Users;
using System;
using System.Collections.Generic;

namespace Myrtus.Clarity.Infrastructure.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");
            builder.HasKey(r => r.Id);

            // Column Mappings
            builder.Property(r => r.Name).HasColumnName("name").IsRequired();
            builder.Property(r => r.IsDefault).HasColumnName("is_default").IsRequired();

            // Data Seeding for Roles
            builder.HasData(
                new
                {
                    Id = Role.Admin.Id,
                    Name = Role.Admin.Name,
                    IsDefault = Role.Admin.IsDefault,
                    CreatedBy = "System",
                    CreatedOnUtc = DateTime.UtcNow,
                },
                new
                {
                    Id = Role.DefaultRole.Id,
                    Name = Role.DefaultRole.Name,
                    IsDefault = Role.DefaultRole.IsDefault,
                    CreatedBy = "System",
                    CreatedOnUtc = DateTime.UtcNow,
                }
            );

            // Configure Many-to-Many Relationship with Permissions
            builder
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "role_permission",
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
                        rp.HasData(
                            new { RoleId = Role.Admin.Id, PermissionId = Permission.UsersRead.Id },
                            new { RoleId = Role.Admin.Id, PermissionId = Permission.UsersCreate.Id },
                            new { RoleId = Role.Admin.Id, PermissionId = Permission.UsersUpdate.Id },
                            new { RoleId = Role.Admin.Id, PermissionId = Permission.UsersDelete.Id },
                            new { RoleId = Role.Admin.Id, PermissionId = Permission.RolesRead.Id },
                            new { RoleId = Role.Admin.Id, PermissionId = Permission.RolesCreate.Id },
                            new { RoleId = Role.Admin.Id, PermissionId = Permission.RolesUpdate.Id },
                            new { RoleId = Role.Admin.Id, PermissionId = Permission.RolesDelete.Id },
                            new { RoleId = Role.Admin.Id, PermissionId = Permission.PermissionsRead.Id },
                            new { RoleId = Role.Admin.Id, PermissionId = Permission.AuditLogsRead.Id },
                            new { RoleId = Role.Admin.Id, PermissionId = Permission.NotificationsRead.Id },
                            new { RoleId = Role.Admin.Id, PermissionId = Permission.NotificationsUpdate.Id },
                            new { RoleId = Role.DefaultRole.Id, PermissionId = Permission.NotificationsRead.Id },
                            new { RoleId = Role.DefaultRole.Id, PermissionId = Permission.NotificationsUpdate.Id },
                            new { RoleId = Role.DefaultRole.Id, PermissionId = Permission.UsersRead.Id }
                        );
                    }
                );

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

                        // Seed Admin User <-> Admin Role
                        ru.HasData(
                            new
                            {
                                RoleId = Role.Admin.Id,
                                UserId = Guid.Parse("55c7f429-0916-4d84-8b76-d45185d89aa7") 
                            },
                            new
                            {
                                RoleId = Role.DefaultRole.Id,
                                UserId = Guid.Parse("55c7f429-0916-4d84-8b76-d45185d89aa7")
                            }
                        );
                    }
                );
        }
    }
}
