using Notification_ServiceAPI.Models.Notifications;

namespace Notification_ServiceAPI.Repositories.Interfaces;

public interface INotificationRepository
{
    Task AddEmailAsync(EmailNotification entity, CancellationToken cancellationToken = default);

    Task AddSmsAsync(SmsNotification entity, CancellationToken cancellationToken = default);

    Task AddPushAsync(PushNotification entity, CancellationToken cancellationToken = default);

    Task AddHistoryAsync(NotificationHistory entity, CancellationToken cancellationToken = default);

    Task<EmailNotification?> GetEmailAsync(Guid notificationId, CancellationToken cancellationToken = default);

    Task<SmsNotification?> GetSmsAsync(Guid notificationId, CancellationToken cancellationToken = default);

    Task<PushNotification?> GetPushAsync(Guid notificationId, CancellationToken cancellationToken = default);

    Task<NotificationHistory?> GetHistoryByIdAsync(Guid notificationId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NotificationHistory>> GetHistoryAsync(string? channel, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}