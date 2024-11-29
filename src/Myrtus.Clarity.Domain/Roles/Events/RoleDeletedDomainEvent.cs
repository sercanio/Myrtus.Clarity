using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Domain.Roles.Events
{
    public sealed record RoleDeletedDomainEvent(Guid RoleId) : IDomainEvent;
}