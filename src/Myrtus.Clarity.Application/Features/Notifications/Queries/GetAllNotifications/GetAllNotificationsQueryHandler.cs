using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Application.Services.Users;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication;
using Myrtus.Clarity.Core.Application.Abstractions.Notification;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.Clarity.Domain.Users;

namespace Myrtus.Clarity.Application.Features.Notifications.Queries.GetAllNotifications
{
    public sealed class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, Result<GetAllNotificationsWithUnreadCountResponse>>
    {
        private readonly INotificationService _notificationService;
        private readonly IUserContext _userContext;
        private readonly IUserService _userService;

        public GetAllNotificationsQueryHandler(INotificationService notificationService, IUserContext userContext, IUserService userService)
        {
            _notificationService = notificationService;
            _userContext = userContext;
            _userService = userService;
        }

        public async Task<Result<GetAllNotificationsWithUnreadCountResponse>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
        {
            User? user = await _userService.GetAsync(
                predicate: user => user.IdentityId == _userContext.IdentityId,
                cancellationToken: cancellationToken);

            if (user is null)
            {
                return Result.NotFound(UserErrors.NotFound.Name);
            }

            List<Notification> notifications = await _notificationService.GetNotificationsByUserIdAsync(user.IdentityId.ToString());

            int unreadCount = notifications.Count(notification => !notification.IsRead);

            List<GetAllNotificationsQueryResponse> paginatedNotifications = notifications
                .OrderByDescending(notification => notification.Timestamp)
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize)
                .Select(notification => new GetAllNotificationsQueryResponse(
                    notification.Id,
                    notification.UserId,
                    notification.User,
                    notification.Action,
                    notification.Entity,
                    notification.EntityId,
                    notification.Timestamp,
                    notification.Details,
                    notification.IsRead
                ))
                .ToList();

            PaginatedList<GetAllNotificationsQueryResponse> paginatedList = new(
                paginatedNotifications,
                notifications.Count,
                request.PageIndex,
                request.PageSize
            );

            var response = new GetAllNotificationsWithUnreadCountResponse(paginatedList, unreadCount);

            return Result.Success(response);
        }
    }
}
