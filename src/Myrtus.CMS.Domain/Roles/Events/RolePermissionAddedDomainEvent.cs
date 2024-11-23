using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Domain.Roles.Events
{
    public sealed record RolePermissionAddedDomainEvent(Guid RoleId, Guid PermissionId) : IDomainEvent;
}