using Notification_ServiceAPI.DTOs.Notifications;

namespace Notification_ServiceAPI.Services.Interfaces;

public interface INotificationService
{
    Task<NotificationDispatchResultDto> SendEmailAsync(SendEmailRequestDto dto, CancellationToken cancellationToken = default);

    Task<NotificationDispatchResultDto> SendSmsAsync(SendSmsRequestDto dto, CancellationToken cancellationToken = default);

    Task<NotificationDispatchResultDto> SendPushAsync(SendPushRequestDto dto, CancellationToken cancellationToken = default);

    Task<NotificationDispatchResultDto> RetryAsync(Guid notificationId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NotificationHistoryDto>> GetHistoryAsync(string? channel, CancellationToken cancellationToken = default);
}