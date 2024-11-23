using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Domain.Users.Events
{
    public sealed record UserRoleRemovedDomainEvent(Guid UserId, Guid RoleId) : IDomainEvent;
}