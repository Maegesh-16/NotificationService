using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Contracts;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController(INotificationDispatchService notificationService) : ControllerBase
{
    [HttpPost("email")]
    [Authorize(Roles = "Admin,Support")]
    public async Task<ActionResult<NotificationDispatchResult>> SendEmail([FromBody] SendEmailRequest request, CancellationToken cancellationToken)
    {
        return Ok(await notificationService.SendEmailAsync(request, cancellationToken));
    }

    [HttpPost("sms")]
    [Authorize(Roles = "Admin,Support")]
    public async Task<ActionResult<NotificationDispatchResult>> SendSms([FromBody] SendSmsRequest request, CancellationToken cancellationToken)
    {
        return Ok(await notificationService.SendSmsAsync(request, cancellationToken));
    }

    [HttpPost("push")]
    [Authorize(Roles = "Admin,Support")]
    public async Task<ActionResult<NotificationDispatchResult>> SendPush([FromBody] SendPushRequest request, CancellationToken cancellationToken)
    {
        return Ok(await notificationService.SendPushAsync(request, cancellationToken));
    }

    [HttpGet("history")]
    public async Task<ActionResult<IReadOnlyCollection<NotificationHistoryDto>>> GetHistory([FromQuery] string? channel, CancellationToken cancellationToken)
    {
        return Ok(await notificationService.GetHistoryAsync(channel, cancellationToken));
    }

    [HttpGet("health")]
    [AllowAnonymous]
    public IActionResult Health() => Ok(new { status = "Notification service is running" });
}
