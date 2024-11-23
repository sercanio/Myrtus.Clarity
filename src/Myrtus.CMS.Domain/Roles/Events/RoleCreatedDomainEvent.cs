using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Roles.Events
{
    public sealed record RoleCreatedDomainEvent(Guid RoleId) : IDomainEvent;
}