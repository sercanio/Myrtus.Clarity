using Myrtus.Clarity.Core.Application.Abstractions.Pagination;

namespace Myrtus.Clarity.Application.Features.Notifications.Queries.GetAllNotifications
{
    public sealed record GetAllNotificationsWithUnreadCountResponse(
        IPaginatedList<GetAllNotificationsQueryResponse> PaginatedNotifications,
        int UnreadCount);
}
