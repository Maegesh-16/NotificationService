using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_ServiceAPI.Models.Notifications;

namespace Notification_ServiceAPI.Data.Configurations;

public class PushNotificationConfiguration : IEntityTypeConfiguration<PushNotification>
{
    public void Configure(EntityTypeBuilder<PushNotification> builder)
    {
        builder.ToTable("PushNotifications");
        builder.HasKey(x => x.NotificationId);
        builder.Property(x => x.RecipientDeviceToken).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(32).IsRequired();
        builder.Property(x => x.FailureReason).HasMaxLength(500);
    }
}