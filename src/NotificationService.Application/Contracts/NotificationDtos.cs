using System.ComponentModel.DataAnnotations;

namespace NotificationService.Application.Contracts;

public sealed record SendEmailRequest(
	[Required, EmailAddress, MaxLength(254)] string RecipientEmail,
	[Required, MaxLength(120)] string Subject,
	[Required, MaxLength(2000)] string Body);

public sealed record SendSmsRequest(
	[Required, MaxLength(20)] string RecipientPhone,
	[Required, MaxLength(500)] string Message);

public sealed record SendPushRequest(
	[Required, MaxLength(255)] string RecipientDeviceToken,
	[Required, MaxLength(120)] string Title,
	[Required, MaxLength(1000)] string Message);

public sealed record NotificationDispatchResult(Guid NotificationId, string Channel, string Recipient, string Status, DateTime SentOn);
public sealed record NotificationHistoryDto(Guid NotificationId, string Channel, string Recipient, string Status, DateTime SentOn);
