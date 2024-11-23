// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Myrtus.Clarity.Infrastructure.SeedData;

namespace Myrtus.Clarity.WebAPI.Extensions.SeedData
{
    internal static class SeedDataExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            await app.SeedPermissionsDataAsync();
            await app.SeedRolesDataAsync();
            await app.SeedRolePermissionsDataAsync();
            Guid adminId = await app.SeedUsersDataAsync();
            await app.SeedRoleUserDataAsync(adminId);
        }
    }
}
