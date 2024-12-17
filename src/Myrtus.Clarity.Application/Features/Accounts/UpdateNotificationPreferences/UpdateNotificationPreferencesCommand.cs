using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Domain.Users.ValueObjects;

namespace Myrtus.Clarity.Application.Features.Accounts.UpdateNotificationPreferences
{
    public sealed record UpdateNotificationPreferencesCommand(
        NotificationPreference NotificationPreference) : ICommand<UpdateNotificationPreferencesCommandResponse>;
}
