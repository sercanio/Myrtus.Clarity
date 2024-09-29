namespace Myrtus.Clarity.Core.Infrastructure.Outbox;

public sealed class OutboxOptions
{
    public int IntervalInSeconds { get; init; }

    public int BatchSize { get; init; }
}
