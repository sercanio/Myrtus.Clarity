using Myrtus.Clarity.Core.Application.Exceptions;

namespace Myrtus.Clarity.Core.Domain.Abstractions;

public record Error(string Code, int? StatusCode, string Name)
{
    public static readonly Error None = new(string.Empty, null, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", 400, "Null value was provided");

    // Validation errors
    public IEnumerable<ValidationError>? Errors { get; init; }

    // Constructor with validation errors
    public Error(string code, int? statusCode, string name, IEnumerable<ValidationError> errors)
        : this(code, statusCode, name)
    {
        Errors = errors?.ToList(); // Ensure non-null initialization
    }
}
