namespace NotificationService.Domain.Entities;

public class NotificationHistory
{
    public Guid NotificationId { get; set; }
    public string Channel { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public string Status { get; set; } = "Queued";
    public DateTime SentOn { get; set; }
}
