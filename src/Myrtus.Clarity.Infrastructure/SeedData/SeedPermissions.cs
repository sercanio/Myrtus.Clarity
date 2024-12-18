using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;

namespace Myrtus.Clarity.Infrastructure.SeedData
{
    public static class SeedPermissions
    {
        public static async Task SeedPermissionsDataAsync(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
            using IDbConnection connection = sqlConnectionFactory.CreateConnection();

            List<object> permissions =
            [
                Permission.UsersRead,
                    Permission.UsersCreate,
                    Permission.UsersUpdate,
                    Permission.UsersDelete,
                    Permission.RolesRead,
                    Permission.RolesCreate,
                    Permission.RolesUpdate,
                    Permission.RolesDelete,
                    Permission.PermissionsRead,
                    Permission.AuditLogsRead,
                    Permission.NotificationsRead
            ];

            const string sql =
                """
                INSERT INTO permissions (id, feature, name, created_by, created_on_utc)
                VALUES (@Id, @Feature, @Name, @CreatedBy, @CreatedOnUtc)
                ON CONFLICT (id) DO NOTHING; -- Avoid duplicate entries
                """;

            await connection.ExecuteAsync(sql, permissions);
        }
    }

}
