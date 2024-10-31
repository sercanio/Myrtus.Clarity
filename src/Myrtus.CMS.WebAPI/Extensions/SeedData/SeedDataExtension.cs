using Myrtus.CMS.Infrastructure.SeedData;

namespace Myrtus.CMS.WebAPI.Extensions.SeedData;

public static class SeedDataExtensions
{
    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        await app.SeedPermissionsDataAsync();
        await app.SeedRolesDataAsync();
        await app.SeedRolePermissionsDataAsync();
        var adminId = await app.SeedUsersDataAsync();
        await app.SeedRoleUserDataAsync(adminId);
    }
}
