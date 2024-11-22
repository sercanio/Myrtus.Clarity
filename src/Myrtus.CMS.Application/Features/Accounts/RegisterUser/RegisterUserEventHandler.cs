using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Domain.Users.Events;

namespace Myrtus.CMS.Application.Features.Accounts.RegisterUser;

internal class RegisterUserEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuditLogService _auditLogService;

    public RegisterUserEventHandler(
        IUserRepository userRepository,
        IAuditLogService auditLogService)
    {
        _userRepository = userRepository;
        _auditLogService = auditLogService;
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetAsync(
            predicate: user => user.Id == notification.UserId,
            cancellationToken: cancellationToken);
        if (user is null)
        {
            return;
        }

        user.CreatedBy = user.Email;

        AuditLog log = new()
        {
            User = user.CreatedBy!,
            Action = UserDomainEvents.Created,
            Entity = user.GetType().Name,
            EntityId = user.Id.ToString(),
            Details = $"User with email '{user.Email}' created."
        };
        await _auditLogService.LogAsync(log);
    }
}
