namespace Notification_ServiceAPI.Services.Dispatch;

public sealed record NotificationDispatchOutcome(bool IsSuccess, string? FailureReason);