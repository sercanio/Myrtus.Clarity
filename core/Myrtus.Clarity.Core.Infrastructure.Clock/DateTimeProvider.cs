using Myrtus.Clarity.Core.Application.Abstractions.Clock;

namespace Myrtus.Clarity.Core.Infrastructure.Clock;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
