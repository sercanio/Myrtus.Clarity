using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Roles.Events
{
    public sealed record RolePermissionRemovedDomainEvent(Guid RoleId, Guid PermissionId) : IDomainEvent;
}