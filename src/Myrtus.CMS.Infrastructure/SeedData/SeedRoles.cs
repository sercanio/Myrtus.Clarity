using System.Data;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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

        // Get the SQL connection factory from the service provider
        ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        // Define the roles to seed
        var registeredRole = Role.Registered;
        var adminRole = Role.Admin;

        // Set the Role Ids for reference if needed
        RegisteredRoleId = registeredRole.Id;
        AdminRoleId = adminRole.Id;

        // SQL statement for inserting role data
        const string sql = """
            INSERT INTO roles (id, name, created_on_utc)
            VALUES (@Id, @Name, @CreatedOnUtc)
            ON CONFLICT (id) DO NOTHING; -- Avoid duplicate entries
            """;

        // Execute the insert operation for each role using Dapper
        connection.Execute(sql, new
        {
            Id = registeredRole.Id,
            Name = registeredRole.Name,
            CreatedOnUtc = DateTime.UtcNow
        });

        connection.Execute(sql, new
        {
            Id = adminRole.Id,
            Name = adminRole.Name,
            CreatedOnUtc = DateTime.UtcNow
        });
    }
}
