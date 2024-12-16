namespace Stocks.API.Exceptions;

public record Details(string Detail, string Title, int StatusCode);

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = ExceptionFormatter.GetStatusCodeFromException(exception);
        var title = exception.GetType().Name;
        var details = exception.Message;
        var instance = httpContext.Request.Path;
        
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = details,
            Status = statusCode,
            Instance = instance,
        };
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}

public static class ExceptionFormatter
{
    public static int GetStatusCodeFromException(Exception exception)
    {
        var exceptionTypeToStatusCode = new Dictionary<Type, int>()
        {
            {typeof(ResourceNotFoundException), StatusCodes.Status404NotFound}
        };

        return exceptionTypeToStatusCode.GetValueOrDefault(exception.GetType(), StatusCodes.Status500InternalServerError);
    }
}