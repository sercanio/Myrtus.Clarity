using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Application.Services.Roles;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Domain.Users.Events;

namespace Myrtus.CMS.Application.Features.Users.Commands.Update.UpdateUserRoles
{
    internal class AddUserRoleEventHandler(
        IUserRepository userRepository,
        IRoleService roleService,
        IAuditLogService auditLogService) : INotificationHandler<UserRoleAddedDomainEvent>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRoleService _roleService = roleService;
        private readonly IAuditLogService _auditLogService = auditLogService;

        public async Task Handle(UserRoleAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetUserByIdAsync(notification.UserId, cancellationToken);

            Role? role = await _roleService.GetAsync(
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
}