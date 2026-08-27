namespace Notification_ServiceAPI.Models.Notifications;

public class EmailNotification
{
    public Guid NotificationId { get; set; }

    public string RecipientEmail { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public string Status { get; set; } = "Queued";

    public int AttemptCount { get; set; }

    public string? FailureReason { get; set; }

    public DateTime? LastAttemptOn { get; set; }

    public DateTime SentOn { get; set; }
}