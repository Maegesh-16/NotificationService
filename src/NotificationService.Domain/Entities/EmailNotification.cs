namespace NotificationService.Domain.Entities;

public class EmailNotification
{
    public Guid NotificationId { get; set; }
    public string RecipientEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Status { get; set; } = "Queued";
    public DateTime SentOn { get; set; }
}
