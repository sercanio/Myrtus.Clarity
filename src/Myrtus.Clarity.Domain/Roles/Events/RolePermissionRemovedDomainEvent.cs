using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Domain.Roles.Events
{
    public sealed record RolePermissionRemovedDomainEvent(Guid RoleId, Guid PermissionId) : IDomainEvent;
}