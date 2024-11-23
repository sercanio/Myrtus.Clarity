using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Application.Services.Roles;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Users;
using Myrtus.Clarity.Domain.Users.Events;

namespace Myrtus.CMS.Application.Features.Users.Commands.Update.UpdateUserRoles
{
    internal class RemoveUserRoleEventHandler(
        IUserRepository userRepository,
        IRoleService roleRepository,
        IAuditLogService auditLogService) : INotificationHandler<UserRoleRemovedDomainEvent>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRoleService _roleService = roleRepository;
        private readonly IAuditLogService _auditLogService = auditLogService;

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
}
