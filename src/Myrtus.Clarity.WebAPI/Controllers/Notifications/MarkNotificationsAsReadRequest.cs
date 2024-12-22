using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.WebAPI.Controllers.Notifications
{
    public sealed record MarkNotificationsAsReadRequest(
        ICollection<Guid> NotificationIds);
}
