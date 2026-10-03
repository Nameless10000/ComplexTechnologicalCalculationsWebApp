using Microsoft.AspNetCore.Diagnostics;

namespace Web.Infrastructure;

public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var (status, code, message) = ApiError.Map(exception);
        logger.LogError(exception, "Request failed with {Code}; trace {TraceId}", code, context.TraceIdentifier);
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(ApiError.Create(context, code, message), cancellationToken);
        return true;
    }
}
