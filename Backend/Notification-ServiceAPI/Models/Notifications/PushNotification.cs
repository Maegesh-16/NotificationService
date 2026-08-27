namespace Notification_ServiceAPI.Models.Notifications;

public class PushNotification
{
    public Guid NotificationId { get; set; }

    public string RecipientDeviceToken { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Status { get; set; } = "Queued";

    public int AttemptCount { get; set; }

    public string? FailureReason { get; set; }

    public DateTime? LastAttemptOn { get; set; }

    public DateTime SentOn { get; set; }
}