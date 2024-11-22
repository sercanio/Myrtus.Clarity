using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using Myrtus.CMS.Domain.Roles;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;

namespace Myrtus.CMS.Infrastructure.SeedData;

public static class SeedPermissions
{
    public static async Task SeedPermissionsDataAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        var permissions = new List<object>
        {
            Permission.UsersRead,
            Permission.UsersCreate,
            Permission.UsersUpdate,
            Permission.UsersDelete,
            Permission.RolesRead,
            Permission.RolesCreate,
            Permission.RolesUpdate,
            Permission.RolesDelete,
            Permission.PermissionsRead
        };

        const string sql = """
            INSERT INTO permissions (id, feature, name, created_by, created_on_utc)
            VALUES (@Id, @Feature, @Name, @CreatedBy, @CreatedOnUtc)
            ON CONFLICT (id) DO NOTHING; -- Avoid duplicate entries
            """;

        connection.Execute(sql, permissions);
    }
}

