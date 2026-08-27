namespace Notification_ServiceAPI.Models.Notifications;

public class SmsNotification
{
    public Guid NotificationId { get; set; }

    public string RecipientPhone { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Status { get; set; } = "Queued";

    public int AttemptCount { get; set; }

    public string? FailureReason { get; set; }

    public DateTime? LastAttemptOn { get; set; }

    public DateTime SentOn { get; set; }
}