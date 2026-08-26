namespace NotificationService.Application.Contracts;

public interface INotificationDispatchService
{
    Task<NotificationDispatchResult> SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken);
    Task<NotificationDispatchResult> SendSmsAsync(SendSmsRequest request, CancellationToken cancellationToken);
    Task<NotificationDispatchResult> SendPushAsync(SendPushRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<NotificationHistoryDto>> GetHistoryAsync(string? channel, CancellationToken cancellationToken);
}
