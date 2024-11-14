using System.Data;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Infrastructure.SeedData;

public static class SeedUsers
{
    internal static Guid AdminId;
    public static async Task<Guid> SeedUsersDataAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        var adminUser = User.CreateWithoutRolesForSeeding(
            "Admin",
            "Admin",
            "admin@email.com");

        adminUser.SetIdentityId("d81eefe5-aa20-4d5c-9a22-6476d2774b1a");
        AdminId = adminUser.Id;

        var adminUserDto = new
        {
            Id = adminUser.Id,
            FirstName = adminUser.FirstName,
            LastName = adminUser.LastName,
            Email = adminUser.Email,
            IdentityId = adminUser.IdentityId,
            CreatedOnUtc = DateTime.UtcNow
        };

        const string userSql = """
            INSERT INTO users (id, first_name, last_name, email, identity_id, created_on_utc)
            VALUES (@Id, @FirstName, @LastName, @Email, @IdentityId, @CreatedOnUtc)
            ON CONFLICT (id) DO NOTHING; -- Avoid duplicate entries
            """;
        connection.Execute(userSql, adminUserDto);

        List<object> roleUsers = new()
        {
            new { RoleId = Role.Registered.Id, UserId = adminUser.Id },
            new { RoleId = Role.Admin.Id, UserId = adminUser.Id }
        };

        const string roleSql = """
            INSERT INTO role_user (roles_id, users_id)
            VALUES(@RoleId, @UserId)
            ON CONFLICT (roles_id, users_id) DO NOTHING;
            """;
        connection.Execute(roleSql, roleUsers);

        return AdminId;
    }
}
