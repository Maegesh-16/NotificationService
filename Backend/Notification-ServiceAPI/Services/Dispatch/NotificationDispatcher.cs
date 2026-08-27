namespace Notification_ServiceAPI.Services.Dispatch;

// This dispatcher is an abstraction point for real email/SMS/push providers.
public class NotificationDispatcher : INotificationDispatcher
{
    public Task<NotificationDispatchOutcome> DispatchAsync(string channel, string recipient, CancellationToken cancellationToken = default)
    {
        if (recipient.Contains("fail", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(new NotificationDispatchOutcome(false, $"{channel} provider rejected recipient '{recipient}'."));
        }

        return Task.FromResult(new NotificationDispatchOutcome(true, null));
    }
}