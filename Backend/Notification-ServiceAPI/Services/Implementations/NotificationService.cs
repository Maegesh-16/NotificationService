using Notification_ServiceAPI.DTOs.Notifications;
using Notification_ServiceAPI.Mappers;
using Notification_ServiceAPI.Models.Notifications;
using Notification_ServiceAPI.Repositories.Interfaces;
using Notification_ServiceAPI.Services.Dispatch;
using Notification_ServiceAPI.Services.Interfaces;

namespace Notification_ServiceAPI.Services.Implementations;

public class NotificationService(INotificationRepository notificationRepository, INotificationDispatcher notificationDispatcher) : INotificationService
{
    public async Task<NotificationDispatchResultDto> SendEmailAsync(SendEmailRequestDto dto, CancellationToken cancellationToken = default)
    {
        var notificationId = Guid.NewGuid();
        var queuedOn = DateTime.UtcNow;
        var dispatchOutcome = await notificationDispatcher.DispatchAsync("Email", dto.RecipientEmail, cancellationToken);

        var status = dispatchOutcome.IsSuccess ? "Sent" : "Failed";
        var sentOn = dispatchOutcome.IsSuccess ? DateTime.UtcNow : queuedOn;
        var attemptOn = DateTime.UtcNow;

        await notificationRepository.AddEmailAsync(new EmailNotification
        {
            NotificationId = notificationId,
            RecipientEmail = dto.RecipientEmail,
            Subject = dto.Subject,
            Body = dto.Body,
            Status = status,
            AttemptCount = 1,
            FailureReason = dispatchOutcome.FailureReason,
            LastAttemptOn = attemptOn,
            SentOn = sentOn
        }, cancellationToken);

        await notificationRepository.AddHistoryAsync(new NotificationHistory
        {
            NotificationId = notificationId,
            Channel = "Email",
            Recipient = dto.RecipientEmail,
            Status = status,
            AttemptCount = 1,
            FailureReason = dispatchOutcome.FailureReason,
            LastAttemptOn = attemptOn,
            SentOn = sentOn
        }, cancellationToken);

        await notificationRepository.SaveChangesAsync(cancellationToken);

        return new NotificationDispatchResultDto(notificationId, "Email", dto.RecipientEmail, status, sentOn, 1, dispatchOutcome.FailureReason);
    }

    public async Task<NotificationDispatchResultDto> SendSmsAsync(SendSmsRequestDto dto, CancellationToken cancellationToken = default)
    {
        var notificationId = Guid.NewGuid();
        var queuedOn = DateTime.UtcNow;
        var dispatchOutcome = await notificationDispatcher.DispatchAsync("SMS", dto.RecipientPhone, cancellationToken);

        var status = dispatchOutcome.IsSuccess ? "Sent" : "Failed";
        var sentOn = dispatchOutcome.IsSuccess ? DateTime.UtcNow : queuedOn;
        var attemptOn = DateTime.UtcNow;

        await notificationRepository.AddSmsAsync(new SmsNotification
        {
            NotificationId = notificationId,
            RecipientPhone = dto.RecipientPhone,
            Message = dto.Message,
            Status = status,
            AttemptCount = 1,
            FailureReason = dispatchOutcome.FailureReason,
            LastAttemptOn = attemptOn,
            SentOn = sentOn
        }, cancellationToken);

        await notificationRepository.AddHistoryAsync(new NotificationHistory
        {
            NotificationId = notificationId,
            Channel = "SMS",
            Recipient = dto.RecipientPhone,
            Status = status,
            AttemptCount = 1,
            FailureReason = dispatchOutcome.FailureReason,
            LastAttemptOn = attemptOn,
            SentOn = sentOn
        }, cancellationToken);

        await notificationRepository.SaveChangesAsync(cancellationToken);

        return new NotificationDispatchResultDto(notificationId, "SMS", dto.RecipientPhone, status, sentOn, 1, dispatchOutcome.FailureReason);
    }

    public async Task<NotificationDispatchResultDto> SendPushAsync(SendPushRequestDto dto, CancellationToken cancellationToken = default)
    {
        var notificationId = Guid.NewGuid();
        var queuedOn = DateTime.UtcNow;
        var dispatchOutcome = await notificationDispatcher.DispatchAsync("Push", dto.RecipientDeviceToken, cancellationToken);

        var status = dispatchOutcome.IsSuccess ? "Sent" : "Failed";
        var sentOn = dispatchOutcome.IsSuccess ? DateTime.UtcNow : queuedOn;
        var attemptOn = DateTime.UtcNow;

        await notificationRepository.AddPushAsync(new PushNotification
        {
            NotificationId = notificationId,
            RecipientDeviceToken = dto.RecipientDeviceToken,
            Title = dto.Title,
            Message = dto.Message,
            Status = status,
            AttemptCount = 1,
            FailureReason = dispatchOutcome.FailureReason,
            LastAttemptOn = attemptOn,
            SentOn = sentOn
        }, cancellationToken);

        await notificationRepository.AddHistoryAsync(new NotificationHistory
        {
            NotificationId = notificationId,
            Channel = "Push",
            Recipient = dto.RecipientDeviceToken,
            Status = status,
            AttemptCount = 1,
            FailureReason = dispatchOutcome.FailureReason,
            LastAttemptOn = attemptOn,
            SentOn = sentOn
        }, cancellationToken);

        await notificationRepository.SaveChangesAsync(cancellationToken);

        return new NotificationDispatchResultDto(notificationId, "Push", dto.RecipientDeviceToken, status, sentOn, 1, dispatchOutcome.FailureReason);
    }

    public async Task<NotificationDispatchResultDto> RetryAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        var history = await notificationRepository.GetHistoryByIdAsync(notificationId, cancellationToken)
            ?? throw new InvalidOperationException($"Notification '{notificationId}' was not found.");

        if (!string.Equals(history.Status, "Failed", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Only failed notifications can be retried.");
        }

        var dispatchOutcome = await notificationDispatcher.DispatchAsync(history.Channel, history.Recipient, cancellationToken);

        history.AttemptCount += 1;
        history.LastAttemptOn = DateTime.UtcNow;
        history.Status = dispatchOutcome.IsSuccess ? "Sent" : "Failed";
        history.FailureReason = dispatchOutcome.FailureReason;
        if (dispatchOutcome.IsSuccess)
        {
            history.SentOn = DateTime.UtcNow;
        }

        switch (history.Channel.ToUpperInvariant())
        {
            case "EMAIL":
                var email = await notificationRepository.GetEmailAsync(notificationId, cancellationToken)
                    ?? throw new InvalidOperationException($"Email notification '{notificationId}' was not found.");
                email.AttemptCount = history.AttemptCount;
                email.LastAttemptOn = history.LastAttemptOn;
                email.Status = history.Status;
                email.FailureReason = history.FailureReason;
                if (dispatchOutcome.IsSuccess)
                {
                    email.SentOn = history.SentOn;
                }
                break;
            case "SMS":
                var sms = await notificationRepository.GetSmsAsync(notificationId, cancellationToken)
                    ?? throw new InvalidOperationException($"SMS notification '{notificationId}' was not found.");
                sms.AttemptCount = history.AttemptCount;
                sms.LastAttemptOn = history.LastAttemptOn;
                sms.Status = history.Status;
                sms.FailureReason = history.FailureReason;
                if (dispatchOutcome.IsSuccess)
                {
                    sms.SentOn = history.SentOn;
                }
                break;
            case "PUSH":
                var push = await notificationRepository.GetPushAsync(notificationId, cancellationToken)
                    ?? throw new InvalidOperationException($"Push notification '{notificationId}' was not found.");
                push.AttemptCount = history.AttemptCount;
                push.LastAttemptOn = history.LastAttemptOn;
                push.Status = history.Status;
                push.FailureReason = history.FailureReason;
                if (dispatchOutcome.IsSuccess)
                {
                    push.SentOn = history.SentOn;
                }
                break;
            default:
                throw new InvalidOperationException($"Unsupported notification channel '{history.Channel}'.");
        }

        await notificationRepository.SaveChangesAsync(cancellationToken);

        return new NotificationDispatchResultDto(
            history.NotificationId,
            history.Channel,
            history.Recipient,
            history.Status,
            history.SentOn,
            history.AttemptCount,
            history.FailureReason);
    }

    public async Task<IReadOnlyList<NotificationHistoryDto>> GetHistoryAsync(string? channel, CancellationToken cancellationToken = default)
    {
        var history = await notificationRepository.GetHistoryAsync(channel, cancellationToken);
        return history.Select(x => x.ToHistoryDto()).ToList();
    }
}