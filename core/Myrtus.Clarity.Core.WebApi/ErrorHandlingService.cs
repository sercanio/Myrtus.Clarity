using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Result;

namespace Myrtus.Clarity.Core.WebApi;

public class ErrorHandlingService : IErrorHandlingService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ErrorHandlingService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult HandleErrorResponse<T>(Result<T> result)
    {
        var errorList = result.Errors.ToList();
        var validationErrors = result.ValidationErrors.Select(ve => ve.ErrorMessage).ToList();
        var combinedErrors = errorList.Concat(validationErrors).ToList();

        var summaryMessage = GetSummaryMessage(result.Status);

        var problemDetails = new ProblemDetails
        {
            Title = GetTitle(result.Status),
            Status = GetHttpStatusCode(result.Status),
            Detail = summaryMessage,
            Instance = _httpContextAccessor.HttpContext?.Request.Path,
            Type = GetTypeUri(result.Status)
        };

        problemDetails.Extensions.Add("errors", combinedErrors);
        problemDetails.Extensions.Add("traceId", _httpContextAccessor.HttpContext?.TraceIdentifier);

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    private string GetSummaryMessage(ResultStatus status) =>
        status switch
        {
            ResultStatus.NotFound => "The requested resource could not be found.",
            ResultStatus.Invalid => "There were validation errors with your request.",
            ResultStatus.Error => "An unexpected error occurred.",
            ResultStatus.Forbidden => "You do not have permission to access this resource.",
            ResultStatus.Unauthorized => "You are not authorized to access this resource.",
            ResultStatus.Conflict => "There was a conflict with the current state of the resource.",
            _ => "An unexpected error occurred."
        };

    private string GetTitle(ResultStatus status) =>
        status switch
        {
            ResultStatus.NotFound => "Resource Not Found",
            ResultStatus.Invalid => "Validation Error",
            ResultStatus.Error => "An Unexpected Error Occurred",
            ResultStatus.Forbidden => "Forbidden",
            ResultStatus.Unauthorized => "Unauthorized Access",
            ResultStatus.Conflict => "Conflict",
            _ => "An Unexpected Error Occurred"
        };

    private int GetHttpStatusCode(ResultStatus status) =>
        status switch
        {
            ResultStatus.NotFound => (int)HttpStatusCode.NotFound,
            ResultStatus.Invalid => (int)HttpStatusCode.BadRequest,
            ResultStatus.Error => (int)HttpStatusCode.InternalServerError,
            ResultStatus.Forbidden => (int)HttpStatusCode.Forbidden,
            ResultStatus.Unauthorized => (int)HttpStatusCode.Unauthorized,
            ResultStatus.Conflict => (int)HttpStatusCode.Conflict,
            _ => (int)HttpStatusCode.InternalServerError
        };

    private string GetTypeUri(ResultStatus status) =>
        status switch
        {
            ResultStatus.NotFound => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
            ResultStatus.Invalid => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            ResultStatus.Error => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            ResultStatus.Forbidden => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
            ResultStatus.Unauthorized => "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
            ResultStatus.Conflict => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
            _ => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
        };
}
