using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Domain.Roles.Events
{
    public sealed record RoleNameUpdatedDomainEvent(Guid RoleId, string OldRoleName) : IDomainEvent;
}