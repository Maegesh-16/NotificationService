using System.ComponentModel.DataAnnotations;

namespace Notification_ServiceAPI.DTOs.Notifications;

public sealed record SendEmailRequestDto(
    [Required, EmailAddress, MaxLength(254)] string RecipientEmail,
    [Required, MaxLength(120)] string Subject,
    [Required, MaxLength(2000)] string Body);

public sealed record SendSmsRequestDto(
    [Required, MaxLength(20)] string RecipientPhone,
    [Required, MaxLength(500)] string Message);

public sealed record SendPushRequestDto(
    [Required, MaxLength(255)] string RecipientDeviceToken,
    [Required, MaxLength(120)] string Title,
    [Required, MaxLength(1000)] string Message);

public sealed record NotificationDispatchResultDto(
    Guid NotificationId,
    string Channel,
    string Recipient,
    string Status,
    DateTime SentOn,
    int AttemptCount,
    string? FailureReason);

public sealed record NotificationHistoryDto(
    Guid NotificationId,
    string Channel,
    string Recipient,
    string Status,
    DateTime SentOn,
    int AttemptCount,
    string? FailureReason,
    DateTime? LastAttemptOn);