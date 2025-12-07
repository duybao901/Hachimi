
using AuthorizationApi.Exceptions;
using System.Text.Json;

namespace AuthorizationApi.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        var response = new
        {
            Title = GetTitle(exception),
            Status = statusCode,
            Detail = exception.Message,
            Errors = GetErrors(exception)
        };

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception)
        => exception switch
        {
            IdentityException.TokenException => StatusCodes.Status401Unauthorized,
            BadRequestException => StatusCodes.Status400BadRequest,
            // FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(Exception exception)
        => exception switch
        {
            DomainException domainException => domainException.Title,
            _ => "Server Error"
        };

    private static List<ValidationError> GetErrors(Exception exception)
    {
        var errors = new List<ValidationError>();

        if (exception is ValidationException validationException)
        {
            errors = validationException.Errors;
        }

        if (exception is IdentityException.UserCreationFailedException identityException)
        {
            foreach (var error in identityException.Errors)
            {
                errors.Add(new ValidationError(error.Code, error.Description));
            }
        }

        return errors;
    }
}
