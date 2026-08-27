using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_ServiceAPI.Models.Notifications;

namespace Notification_ServiceAPI.Data.Configurations;

public class SmsNotificationConfiguration : IEntityTypeConfiguration<SmsNotification>
{
    public void Configure(EntityTypeBuilder<SmsNotification> builder)
    {
        builder.ToTable("SmsNotifications");
        builder.HasKey(x => x.NotificationId);
        builder.Property(x => x.RecipientPhone).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(32).IsRequired();
        builder.Property(x => x.FailureReason).HasMaxLength(500);
    }
}