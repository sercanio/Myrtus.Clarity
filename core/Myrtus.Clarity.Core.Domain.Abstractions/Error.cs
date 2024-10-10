namespace Myrtus.Clarity.Core.Domain.Abstractions;

public record Error(string Code, int? StatusCode, string Name)
{
    public static readonly Error None = new(string.Empty, null, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", 400, "Null value was provided");
}
