using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Roles.Events;

public sealed record RolePermissionAddedDomainEvent(Guid RoleId, Guid PermissionId) : IDomainEvent;