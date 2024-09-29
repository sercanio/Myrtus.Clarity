using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Infrastructure.Authorization;

internal sealed class UserRolesResponse
{
    public Guid UserId { get; init; }

    public List<Role> Roles { get; init; } = [];
}
