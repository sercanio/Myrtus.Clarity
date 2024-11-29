using Myrtus.Clarity.Domain.Roles;

namespace Myrtus.Clarity.Infrastructure.Authorization
{
    internal sealed class UserRolesResponse
    {
        public Guid UserId { get; init; }
        public ICollection<Role> Roles { get; init; } = [];
    }
}
