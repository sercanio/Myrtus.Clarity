using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Domain.Users.Events
{
    public sealed record UserRoleAddedDomainEvent(Guid UserId, Guid RoleId) : IDomainEvent;
}