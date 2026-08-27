using Notification_ServiceAPI.DTOs.Notifications;
using Notification_ServiceAPI.Models.Notifications;

namespace Notification_ServiceAPI.Mappers;

public static class NotificationMappingExtensions
{
    public static NotificationHistoryDto ToHistoryDto(this NotificationHistory entity)
    {
        return new NotificationHistoryDto(
            entity.NotificationId,
            entity.Channel,
            entity.Recipient,
            entity.Status,
            entity.SentOn,
            entity.AttemptCount,
            entity.FailureReason,
            entity.LastAttemptOn);
    }
}