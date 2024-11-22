using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Infrastructure.SeedData;

public static class SeedRoles
{
    internal static Guid RegisteredRoleId;
    internal static Guid AdminRoleId;

    public static async Task SeedRolesDataAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        var registeredRole = Role.DefaultRole;
        var adminRole = Role.Admin;

        RegisteredRoleId = registeredRole.Id;
        AdminRoleId = adminRole.Id;

        const string sql = """
            INSERT INTO roles (id, name, is_default, created_by, created_on_utc)
            VALUES (@Id, @Name, @IsDefault, @CreatedBy, @CreatedOnUtc)
            ON CONFLICT (id) DO NOTHING; -- Avoid duplicate entries
            """;

        connection.Execute(sql, new
        {
            Id = registeredRole.Id,
            Name = registeredRole.Name,
            IsDefault = true,
            CreatedBy = registeredRole.CreatedBy,
            CreatedOnUtc = DateTime.UtcNow
        });

        connection.Execute(sql, new
        {
            Id = adminRole.Id,
            Name = adminRole.Name,
            IsDefault = false,
            CreatedBy = adminRole.CreatedBy,
            CreatedOnUtc = DateTime.UtcNow
        });
    }
}
