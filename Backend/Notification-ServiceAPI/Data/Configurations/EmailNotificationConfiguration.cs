using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_ServiceAPI.Models.Notifications;

namespace Notification_ServiceAPI.Data.Configurations;

public class EmailNotificationConfiguration : IEntityTypeConfiguration<EmailNotification>
{
    public void Configure(EntityTypeBuilder<EmailNotification> builder)
    {
        builder.ToTable("EmailNotifications");
        builder.HasKey(x => x.NotificationId);
        builder.Property(x => x.RecipientEmail).HasMaxLength(254).IsRequired();
        builder.Property(x => x.Subject).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Body).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(32).IsRequired();
        builder.Property(x => x.FailureReason).HasMaxLength(500);
    }
}