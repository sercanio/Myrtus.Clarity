using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;

namespace Myrtus.Clarity.Infrastructure.SeedData
{
    public static class SeedRolePermissions
    {
        public static async Task SeedRolePermissionsDataAsync(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
            using IDbConnection connection = sqlConnectionFactory.CreateConnection();

            List<object> rolePermissions =
                [
                    new { RoleId = Role.DefaultRole.Id, PermissionId = Permission.UsersRead.Id },
                    new { RoleId = Role.Admin.Id, PermissionId = Permission.UsersRead.Id },
                    new { RoleId = Role.Admin.Id, PermissionId = Permission.UsersCreate.Id },
                    new { RoleId = Role.Admin.Id, PermissionId = Permission.UsersUpdate.Id },
                    new { RoleId = Role.Admin.Id, PermissionId = Permission.UsersDelete.Id },
                    new { RoleId = Role.Admin.Id, PermissionId = Permission.RolesRead.Id },
                    new { RoleId = Role.Admin.Id, PermissionId = Permission.RolesCreate.Id },
                    new { RoleId = Role.Admin.Id, PermissionId = Permission.RolesUpdate.Id },
                    new { RoleId = Role.Admin.Id, PermissionId = Permission.RolesDelete.Id },
                    new { RoleId = Role.Admin.Id, PermissionId = Permission.PermissionsRead.Id },
                    new { RoleId = Role.Admin.Id, PermissionId = Permission.AuditLogsRead.Id }
                ];

            const string sql =
                    """
                    INSERT INTO permission_role (roles_id, permissions_id)
                    VALUES (@RoleId, @PermissionId)
                    ON CONFLICT (roles_id, permissions_id) DO NOTHING; -- Avoid duplicate entries
                    """;

            await connection.ExecuteAsync(sql, rolePermissions);
        }
    }
}
