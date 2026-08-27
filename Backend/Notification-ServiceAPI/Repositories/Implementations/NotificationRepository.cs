using Microsoft.EntityFrameworkCore;
using Notification_ServiceAPI.Data;
using Notification_ServiceAPI.Models.Notifications;
using Notification_ServiceAPI.Repositories.Interfaces;

namespace Notification_ServiceAPI.Repositories.Implementations;

public class NotificationRepository(NotificationDbContext dbContext) : INotificationRepository
{
    public async Task AddEmailAsync(EmailNotification entity, CancellationToken cancellationToken = default)
    {
        await dbContext.EmailNotifications.AddAsync(entity, cancellationToken);
    }

    public async Task AddSmsAsync(SmsNotification entity, CancellationToken cancellationToken = default)
    {
        await dbContext.SmsNotifications.AddAsync(entity, cancellationToken);
    }

    public async Task AddPushAsync(PushNotification entity, CancellationToken cancellationToken = default)
    {
        await dbContext.PushNotifications.AddAsync(entity, cancellationToken);
    }

    public async Task AddHistoryAsync(NotificationHistory entity, CancellationToken cancellationToken = default)
    {
        await dbContext.NotificationHistories.AddAsync(entity, cancellationToken);
    }

    public async Task<EmailNotification?> GetEmailAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.EmailNotifications.FirstOrDefaultAsync(x => x.NotificationId == notificationId, cancellationToken);
    }

    public async Task<SmsNotification?> GetSmsAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.SmsNotifications.FirstOrDefaultAsync(x => x.NotificationId == notificationId, cancellationToken);
    }

    public async Task<PushNotification?> GetPushAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.PushNotifications.FirstOrDefaultAsync(x => x.NotificationId == notificationId, cancellationToken);
    }

    public async Task<NotificationHistory?> GetHistoryByIdAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.NotificationHistories.FirstOrDefaultAsync(x => x.NotificationId == notificationId, cancellationToken);
    }

    public async Task<IReadOnlyList<NotificationHistory>> GetHistoryAsync(string? channel, CancellationToken cancellationToken = default)
    {
        var query = dbContext.NotificationHistories.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(channel))
        {
            query = query.Where(x => x.Channel == channel);
        }

        return await query
            .OrderByDescending(x => x.SentOn)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}