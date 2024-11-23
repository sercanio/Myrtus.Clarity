using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Infrastructure.Authorization
{
    internal sealed class UserRolesResponse
    {
        public Guid UserId { get; init; }
        public ICollection<Role> Roles { get; init; } = [];
    }
}
