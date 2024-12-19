using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Myrtus.Clarity.Core.Application.Abstractions.Localization.Services;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Myrtus.Clarity.WebAPI.Middleware
{
    public sealed class ExceptionHandlingMiddleware(
                                RequestDelegate next,
                                ILogger<ExceptionHandlingMiddleware> logger,
                                ILocalizationService localizationService)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
        private readonly ILocalizationService _localizationService = localizationService;

        private static readonly Action<ILogger, string, Exception> _logError =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId(1, nameof(ExceptionHandlingMiddleware)),
                "Exception occurred: {Message}");

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logError(_logger, exception.Message, exception);

                var language = GetLanguageFromHeader(context);
                ExceptionDetails exceptionDetails = GetExceptionDetails(exception, language);

                ProblemDetails problemDetails = new()
                {
                    Status = exceptionDetails.Status,
                    Type = exceptionDetails.Type,
                    Title = exceptionDetails.Title,
                    Detail = exceptionDetails.Detail,
                    Instance = context.Request.Path
                };

                if (exceptionDetails.Errors is not null)
                {
                    problemDetails.Extensions["errors"] = exceptionDetails.Errors;
                }

                context.Response.StatusCode = exceptionDetails.Status;
                context.Response.ContentType = "application/json; charset=utf-8";

                var options = new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };
                var json = JsonSerializer.Serialize(problemDetails, options);
                await context.Response.WriteAsync(json, Encoding.UTF8);
            }
        }

        private string GetLanguageFromHeader(HttpContext context)
        {
            var acceptLanguageHeader = context.Request.Headers["Accept-Language"].ToString();
            if (!string.IsNullOrEmpty(acceptLanguageHeader))
            {
                var languages = acceptLanguageHeader.Split(',');
                if (languages.Length > 0)
                {
                    return languages[0];
                }
            }
            return CultureInfo.CurrentCulture.Name;
        }

        private ExceptionDetails GetExceptionDetails(Exception exception, string language)
        {
            return exception switch
            {
                ValidationException validationException => new ExceptionDetails(
                    StatusCodes.Status400BadRequest,
                    "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                    "Validation error",
                    _localizationService.GetLocalizedString("Errors.Validation", language),
                    validationException.Errors),
                _ => new ExceptionDetails(
                    StatusCodes.Status500InternalServerError,
                    "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                    "Server error",
                    _localizationService.GetLocalizedString("Errors.ServerError", language),
                    null)
            };
        }

        internal sealed record ExceptionDetails(
            int Status,
            string Type,
            string Title,
            string Detail,
            IEnumerable<object>? Errors);
    }
}
