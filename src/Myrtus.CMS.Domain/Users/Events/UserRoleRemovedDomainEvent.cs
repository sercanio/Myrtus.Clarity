using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Users.Events
{
    public sealed record UserRoleRemovedDomainEvent(Guid UserId, Guid RoleId) : IDomainEvent;
}