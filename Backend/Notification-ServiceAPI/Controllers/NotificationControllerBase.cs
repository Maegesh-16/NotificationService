using Microsoft.AspNetCore.Mvc;

namespace Notification_ServiceAPI.Controllers;

[ApiController]
public abstract class NotificationControllerBase : ControllerBase
{
    protected ActionResult HandleInvalidOperation(InvalidOperationException exception)
    {
        return BadRequest(new ProblemDetails
        {
            Title = "Notification service validation failed.",
            Detail = exception.Message,
            Status = StatusCodes.Status400BadRequest
        });
    }
}