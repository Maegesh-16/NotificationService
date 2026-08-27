using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification_ServiceAPI.Authentication;
using Notification_ServiceAPI.DTOs.Notifications;
using Notification_ServiceAPI.Services.Interfaces;

namespace Notification_ServiceAPI.Controllers;

[Route("api/notifications")]
[Authorize(Policy = NotificationServicePolicies.NotificationRead)]
public class NotificationsController(INotificationService notificationService) : NotificationControllerBase
{
    [HttpGet("history")]
    public async Task<ActionResult<IReadOnlyList<NotificationHistoryDto>>> GetHistoryAsync(
        [FromQuery] string? channel,
        CancellationToken cancellationToken)
    {
        var notifications = await notificationService.GetHistoryAsync(channel, cancellationToken);
        return Ok(notifications);
    }

    [HttpPost("email")]
    [Authorize(Policy = NotificationServicePolicies.NotificationSend)]
    public async Task<ActionResult<NotificationDispatchResultDto>> SendEmailAsync(
        [FromBody] SendEmailRequestDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await notificationService.SendEmailAsync(dto, cancellationToken));
        }
        catch (InvalidOperationException exception)
        {
            return HandleInvalidOperation(exception);
        }
    }

    [HttpPost("sms")]
    [Authorize(Policy = NotificationServicePolicies.NotificationSend)]
    public async Task<ActionResult<NotificationDispatchResultDto>> SendSmsAsync(
        [FromBody] SendSmsRequestDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await notificationService.SendSmsAsync(dto, cancellationToken));
        }
        catch (InvalidOperationException exception)
        {
            return HandleInvalidOperation(exception);
        }
    }

    [HttpPost("push")]
    [Authorize(Policy = NotificationServicePolicies.NotificationSend)]
    public async Task<ActionResult<NotificationDispatchResultDto>> SendPushAsync(
        [FromBody] SendPushRequestDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await notificationService.SendPushAsync(dto, cancellationToken));
        }
        catch (InvalidOperationException exception)
        {
            return HandleInvalidOperation(exception);
        }
    }

    [HttpPost("{notificationId:guid}/retry")]
    [Authorize(Policy = NotificationServicePolicies.NotificationAdmin)]
    public async Task<ActionResult<NotificationDispatchResultDto>> RetryAsync(Guid notificationId, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await notificationService.RetryAsync(notificationId, cancellationToken));
        }
        catch (InvalidOperationException exception)
        {
            return HandleInvalidOperation(exception);
        }
    }
}