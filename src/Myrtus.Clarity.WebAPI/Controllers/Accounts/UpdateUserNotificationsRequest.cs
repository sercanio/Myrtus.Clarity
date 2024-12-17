namespace Myrtus.Clarity.WebAPI.Controllers.Accounts
{
    public sealed record UpdateUserNotificationsRequest(
        bool InAppNotification,
        bool EmailNotification,
        bool PushNotification);
}
