using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Contracts;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Persistence;

namespace NotificationService.Infrastructure.Services;

public class NotificationDispatchService(NotificationDbContext dbContext) : INotificationDispatchService
{
    public async Task<NotificationDispatchResult> SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken)
    {
        var notificationId = Guid.NewGuid();
        var sentOn = DateTime.UtcNow;

        var email = new EmailNotification
        {
            NotificationId = notificationId,
            RecipientEmail = request.RecipientEmail,
            Subject = request.Subject,
            Body = request.Body,
            Status = "Sent",
            SentOn = sentOn
        };

        dbContext.EmailNotifications.Add(email);
        dbContext.NotificationHistory.Add(new NotificationHistory
        {
            NotificationId = notificationId,
            Channel = "Email",
            Recipient = request.RecipientEmail,
            Status = "Sent",
            SentOn = sentOn
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        return new NotificationDispatchResult(notificationId, "Email", request.RecipientEmail, "Sent", sentOn);
    }

    public async Task<NotificationDispatchResult> SendSmsAsync(SendSmsRequest request, CancellationToken cancellationToken)
    {
        var notificationId = Guid.NewGuid();
        var sentOn = DateTime.UtcNow;

        var sms = new SmsNotification
        {
            NotificationId = notificationId,
            RecipientPhone = request.RecipientPhone,
            Message = request.Message,
            Status = "Sent",
            SentOn = sentOn
        };

        dbContext.SmsNotifications.Add(sms);
        dbContext.NotificationHistory.Add(new NotificationHistory
        {
            NotificationId = notificationId,
            Channel = "SMS",
            Recipient = request.RecipientPhone,
            Status = "Sent",
            SentOn = sentOn
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        return new NotificationDispatchResult(notificationId, "SMS", request.RecipientPhone, "Sent", sentOn);
    }

    public async Task<NotificationDispatchResult> SendPushAsync(SendPushRequest request, CancellationToken cancellationToken)
    {
        var notificationId = Guid.NewGuid();
        var sentOn = DateTime.UtcNow;

        var push = new PushNotification
        {
            NotificationId = notificationId,
            RecipientDeviceToken = request.RecipientDeviceToken,
            Title = request.Title,
            Message = request.Message,
            Status = "Sent",
            SentOn = sentOn
        };

        dbContext.PushNotifications.Add(push);
        dbContext.NotificationHistory.Add(new NotificationHistory
        {
            NotificationId = notificationId,
            Channel = "Push",
            Recipient = request.RecipientDeviceToken,
            Status = "Sent",
            SentOn = sentOn
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        return new NotificationDispatchResult(notificationId, "Push", request.RecipientDeviceToken, "Sent", sentOn);
    }

    public async Task<IReadOnlyCollection<NotificationHistoryDto>> GetHistoryAsync(string? channel, CancellationToken cancellationToken)
    {
        var query = dbContext.NotificationHistory.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(channel))
        {
            query = query.Where(x => x.Channel == channel);
        }

        return await query
            .OrderByDescending(x => x.SentOn)
            .Select(x => new NotificationHistoryDto(x.NotificationId, x.Channel, x.Recipient, x.Status, x.SentOn))
            .ToListAsync(cancellationToken);
    }
}
