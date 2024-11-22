using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Roles.Events;

public sealed record RoleNameUpdatedDomainEvent(Guid RoleId, string OldRoleName) : IDomainEvent;