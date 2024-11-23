using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Domain.Roles.Events
{
    public sealed record RoleCreatedDomainEvent(Guid RoleId) : IDomainEvent;
}