﻿using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Myrtus.Clarity.Domain.Roles;

namespace Myrtus.Clarity.Infrastructure.SeedData
{
    public static class SeedRoleUser
    {
        public static async Task SeedRoleUserDataAsync(this IApplicationBuilder app, Guid adminId)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
            using IDbConnection connection = sqlConnectionFactory.CreateConnection();

            List<object> roleUsers =
                [
                    new
                    {
                        RoleId = Role.DefaultRole.Id,
                        UserId = adminId
                    },
                    new
                    {
                        RoleId = Role.Admin.Id,
                        UserId = adminId
                    }
                ];

            const string sql =
                """
                INSERT INTO role_user (roles_id, users_id)
                VALUES(@RoleId, @UserId)
                ON CONFLICT (roles_id, users_id) DO NOTHING;
                """;

            await connection.ExecuteAsync(sql, roleUsers);
        }
    }
}
