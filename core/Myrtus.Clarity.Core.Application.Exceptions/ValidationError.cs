namespace Myrtus.Clarity.Core.Application.Exceptions;

public sealed record ValidationError(string PropertyName, string ErrorMessage);
