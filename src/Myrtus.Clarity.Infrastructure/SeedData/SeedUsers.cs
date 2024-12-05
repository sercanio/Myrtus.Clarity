using System.Data;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.Clarity.Domain.Users;
using Myrtus.Clarity.Domain.Roles;

namespace Myrtus.Clarity.Infrastructure.SeedData
{
    public static class SeedUsers
    {
        internal static Guid AdminId { get; private set; }

        static SeedUsers()
        {
            AdminId = Guid.Empty;
        }

        public static async Task<Guid> SeedUsersDataAsync(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
            using IDbConnection connection = sqlConnectionFactory.CreateConnection();

            User adminUser = User.CreateWithoutRolesForSeeding(
                "Admin",
                "Admin",
                "admin@email.com");

            adminUser.SetIdentityId("5a7e3a65-0515-45a7-8c4c-f17a03efd851");
            AdminId = adminUser.Id;

            var adminUserDto = new
            {
                adminUser.Id,
                adminUser.FirstName,
                adminUser.LastName,
                adminUser.Email,
                adminUser.IdentityId,
                adminUser.CreatedBy,
                CreatedOnUtc = DateTime.UtcNow
            };

            const string userSql =
                """
                INSERT INTO users (id, first_name, last_name, email, identity_id, created_by, created_on_utc)
                VALUES (@Id, @FirstName, @LastName, @Email, @IdentityId, @CreatedBy, @CreatedOnUtc)
                ON CONFLICT (id) DO NOTHING; -- Avoid duplicate entries
                """;
            await connection.ExecuteAsync(userSql, adminUserDto);

            List<object> roleUsers =
                [
                        new { RoleId = Role.DefaultRole.Id, UserId = adminUser.Id },
                        new { RoleId = Role.Admin.Id, UserId = adminUser.Id }
                ];

            const string roleSql =
                """
                INSERT INTO role_user (roles_id, users_id)
                VALUES(@RoleId, @UserId)
                ON CONFLICT (roles_id, users_id) DO NOTHING;
                """;
            await connection.ExecuteAsync(roleSql, roleUsers);

            return AdminId;
        }
    }
}
