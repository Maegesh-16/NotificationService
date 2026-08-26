namespace NotificationService.Domain.Entities;

public class PushNotification
{
    public Guid NotificationId { get; set; }
    public string RecipientDeviceToken { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = "Queued";
    public DateTime SentOn { get; set; }
}
