using System.Text.Json;
using Afama.Go.Api.Host.Common;

namespace Afama.Go.Api.Host.Infrastructure;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, errorCode) = GetErrorDetails(exception);

        var errorResponse = CreateErrorResponse(exception, context);

        _logger.LogError(exception,
            "Unhandled exception for {Method} {Path} by {User}. Error: {ErrorCode}",
            context.Request.Method,
            context.Request.Path,
            context.User.Identity?.Name ?? "Anonymous",
            errorCode);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = ShouldIncludeStackTrace()
        };

        var json = JsonSerializer.Serialize(errorResponse, options);
        await context.Response.WriteAsync(json);
    }

    private object CreateErrorResponse(Exception exception, HttpContext context)
    {
        var (statusCode, errorCode) = GetErrorDetails(exception);

        // Handle validation exceptions with detailed field errors
        if (exception is Application.Common.Exceptions.ValidationException validationException)
        {
            return new ValidationErrorResponse
            {
                Message = "One or more validation errors occurred.",
                Code = errorCode,
                Path = context.Request.Path,
                Method = context.Request.Method,
                Timestamp = DateTime.UtcNow,
                RequestId = context.TraceIdentifier,
                Errors = validationException.Errors,
                StackTrace = ShouldIncludeStackTrace() ? exception.StackTrace : null
            };
        }

        // Handle FluentValidation exceptions
        if (exception is FluentValidation.ValidationException fluentValidationException)
        {
            var errors = fluentValidationException.Errors
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

            return new ValidationErrorResponse
            {
                Message = "One or more validation errors occurred.",
                Code = errorCode,
                Path = context.Request.Path,
                Method = context.Request.Method,
                Timestamp = DateTime.UtcNow,
                RequestId = context.TraceIdentifier,
                Errors = errors,
                StackTrace = ShouldIncludeStackTrace() ? exception.StackTrace : null
            };
        }

        // Default error response for other exceptions
        return new ErrorResponse
        {
            Message = GetUserFriendlyMessage(exception),
            Code = errorCode,
            Path = context.Request.Path,
            Method = context.Request.Method,
            Timestamp = DateTime.UtcNow,
            RequestId = context.TraceIdentifier,
            StackTrace = ShouldIncludeStackTrace() ? exception.StackTrace : null
        };
    }

    private bool ShouldIncludeStackTrace()
    {
        return _environment.IsLocal() || _environment.IsDevelopment() || _environment.IsStaging();
    }

    private static (int statusCode, string errorCode) GetErrorDetails(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => (400, "InvalidArgument"),
            ArgumentException => (400, "InvalidArgument"),
            InvalidOperationException => (400, "InvalidOperation"),
            FluentValidation.ValidationException => (400, "ValidationFailed"),
            Application.Common.Exceptions.ValidationException => (400, "ValidationFailed"),
            UnauthorizedAccessException => (401, "Unauthorized"),
            KeyNotFoundException => (404, "NotFound"),
            NotImplementedException => (501, "NotImplemented"),
            TimeoutException => (408, "Timeout"),
            TaskCanceledException => (408, "Timeout"),
            NotFoundException => (404, "NotFound"),
            _ => (500, "InternalError")
        };
    }

    private static string GetUserFriendlyMessage(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => "Required parameter is missing.",
            ArgumentException => "Invalid request parameters.",
            InvalidOperationException => "The requested operation is not valid.",
            FluentValidation.ValidationException => "One or more validation errors occurred.",
            Application.Common.Exceptions.ValidationException => "One or more validation errors occurred.",
            UnauthorizedAccessException => "Access denied.",
            KeyNotFoundException => "The requested resource was not found.",
            NotImplementedException => "This feature is not yet implemented.",
            TimeoutException => "The request timed out.",
            TaskCanceledException => "The request was cancelled or timed out.",
            NotFoundException => "The requested resource was not found.",
            _ => "An internal server error occurred. Please try again later."
        };
    }
}

public record ErrorResponse
{
    public string Message { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
    public string Method { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
    public string RequestId { get; init; } = string.Empty;
    public string? StackTrace { get; init; }
}

public record ValidationErrorResponse : ErrorResponse
{
    public IDictionary<string, string[]> Errors { get; init; } = new Dictionary<string, string[]>();
}
