namespace NotificationService.Domain.Entities;

public class SmsNotification
{
    public Guid NotificationId { get; set; }
    public string RecipientPhone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = "Queued";
    public DateTime SentOn { get; set; }
}
