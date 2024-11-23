using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Services.Roles;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Domain.Users.Events;

namespace Myrtus.CMS.Application.Features.Users.Commands.Update.UpdateUserRoles;

internal class RemoveUserRoleEventHandler : INotificationHandler<UserRoleRemovedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleService _roleService;
    private readonly IAuditLogService _auditLogService;

    public RemoveUserRoleEventHandler(
        IUserRepository userRepository,
        IRoleService roleRepository,
        IAuditLogService auditLogService)
    {
        _userRepository = userRepository;
        _roleService = roleRepository;
        _auditLogService = auditLogService;
    }

    public async Task Handle(UserRoleRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetUserByIdAsync(notification.UserId, cancellationToken);

        Role? role = await _roleService.GetAsync(
            predicate: role => role.Id == notification.RoleId,
            includeSoftDeleted: true,
            cancellationToken: cancellationToken);

        AuditLog log = new()
        {
            User = user!.UpdatedBy!,
            Action = UserDomainEvents.RemovedRole,
            Entity = user.GetType().Name,
            EntityId = user.Id.ToString(),
            Details = $"{user.GetType().Name} '{user.Email}' has been revoked the role '{role!.Name}'."
        };
        await _auditLogService.LogAsync(log);
    }
}
