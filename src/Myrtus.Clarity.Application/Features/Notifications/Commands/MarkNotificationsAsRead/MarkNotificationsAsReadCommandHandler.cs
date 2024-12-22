using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Notification;

namespace Myrtus.Clarity.Application.Features.Notifications.Commands.MarkNotificationsAsRead
{
    public sealed class MarkNotificationsAsReadCommandHandler : ICommandHandler<MarkNotificationsAsReadCommand, MarkNotificationsAsReadCommandResponse>
    {
        private readonly INotificationService _notificationService;
        private readonly IUserContext _userContext;

        public MarkNotificationsAsReadCommandHandler(
            INotificationService notificationService, 
            IUserContext userContext)
        {
            _notificationService = notificationService;
            _userContext = userContext;
        }

        public async Task<Result<MarkNotificationsAsReadCommandResponse>> Handle(
            MarkNotificationsAsReadCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _userContext.IdentityId.ToString();
            await _notificationService.MarkNotificationsAsReadAsync(userId,cancellationToken);

            MarkNotificationsAsReadCommandResponse response = new();

            return Result.Success(response);
        }
    }
}
