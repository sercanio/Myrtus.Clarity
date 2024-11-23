using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Infrastructure.SeedData
{
    public static class SeedRoles
    {
        internal static readonly Guid RegisteredRoleId = Role.DefaultRole.Id;
        internal static readonly Guid AdminRoleId = Role.Admin.Id;

        public static async Task SeedRolesDataAsync(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
            using IDbConnection connection = sqlConnectionFactory.CreateConnection();

            Role registeredRole = Role.DefaultRole;
            Role adminRole = Role.Admin;

            const string sql =
                """
                    INSERT INTO roles (id, name, is_default, created_by, created_on_utc)
                    VALUES (@Id, @Name, @IsDefault, @CreatedBy, @CreatedOnUtc)
                    ON CONFLICT (id) DO NOTHING; -- Avoid duplicate entries
                    """;

            await connection.ExecuteAsync(sql, new
            {
                registeredRole.Id,
                registeredRole.Name,
                IsDefault = true,
                registeredRole.CreatedBy,
                CreatedOnUtc = DateTime.UtcNow
            });

            await connection.ExecuteAsync(sql, new
            {
                adminRole.Id,
                adminRole.Name,
                IsDefault = false,
                adminRole.CreatedBy,
                CreatedOnUtc = DateTime.UtcNow
            });
        }
    }
}
