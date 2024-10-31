using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using Myrtus.CMS.Domain.Roles;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;

namespace Myrtus.CMS.Infrastructure.SeedData;

public static class SeedRolePermissions
{
    public static async Task SeedRolePermissionsDataAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        var rolePermissions = new List<object>
        {
            new { RoleId = Role.Registered.Id, PermissionId = Permission.UsersRead.Id },
            new { RoleId = Role.Admin.Id, PermissionId = Permission.UsersRead.Id },
            new { RoleId = Role.Admin.Id, PermissionId = Permission.UsersCreate.Id },
            new { RoleId = Role.Admin.Id, PermissionId = Permission.UsersUpdate.Id },
            new { RoleId = Role.Admin.Id, PermissionId = Permission.UsersDelete.Id },
            new { RoleId = Role.Admin.Id, PermissionId = Permission.RolesRead.Id },
            new { RoleId = Role.Admin.Id, PermissionId = Permission.RolesCreate.Id },
            new { RoleId = Role.Admin.Id, PermissionId = Permission.RolesUpdate.Id },
            new { RoleId = Role.Admin.Id, PermissionId = Permission.RolesDelete.Id },
            new { RoleId = Role.Admin.Id, PermissionId = Permission.PermissionsRead.Id }
        };

        const string sql = """
            INSERT INTO permission_role (roles_id, permissions_id)
            VALUES (@RoleId, @PermissionId)
            ON CONFLICT (roles_id, permissions_id) DO NOTHING; -- Avoid duplicate entries
        """;

        connection.Execute(sql, rolePermissions);
    }
}
