using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace NotificationService.API.Middleware;

public class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error while processing request");
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Validation error", ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception while processing request");
            var detail = environment.IsDevelopment()
                ? ex.Message
                : "An unexpected error occurred.";
            Console.WriteLine(ex);
            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, "Internal server error", detail);
        }
    }

    private static Task WriteErrorAsync(HttpContext context, int statusCode, string title, string detail)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = new
        {
            title,
            status = statusCode,
            detail,
            traceId = context.TraceIdentifier
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
