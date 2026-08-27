namespace Notification_ServiceAPI.Services.Dispatch;

public interface INotificationDispatcher
{
    Task<NotificationDispatchOutcome> DispatchAsync(string channel, string recipient, CancellationToken cancellationToken = default);
}