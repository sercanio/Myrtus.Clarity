using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Domain.Users.Events;

namespace Myrtus.CMS.Application.Features.Users.Commands.Update.UpdateUserRoles;

internal class AddUserRoleEventHandler : INotificationHandler<UserRoleAddedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IAuditLogService _auditLogService;

    public AddUserRoleEventHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IAuditLogService auditLogService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _auditLogService = auditLogService;
    }

    public async Task Handle(UserRoleAddedDomainEvent notification, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetUserByIdAsync(notification.UserId, cancellationToken);

        Role? role = await _roleRepository.GetAsync(
            predicate: role => role.Id == notification.RoleId,
            cancellationToken: cancellationToken);

        AuditLog log = new()
        {
            User = user!.UpdatedBy!,
            Action = UserDomainEvents.AddedRole,
            Entity = user.GetType().Name,
            EntityId = user.Id.ToString(),
            Details = $"{user.GetType().Name} '{user.Email}' has been granted a new role '{role!.Name}'."
        };
        await _auditLogService.LogAsync(log);
    }
}