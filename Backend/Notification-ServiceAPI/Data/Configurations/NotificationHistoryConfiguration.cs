using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification_ServiceAPI.Models.Notifications;

namespace Notification_ServiceAPI.Data.Configurations;

public class NotificationHistoryConfiguration : IEntityTypeConfiguration<NotificationHistory>
{
    public void Configure(EntityTypeBuilder<NotificationHistory> builder)
    {
        builder.ToTable("NotificationHistories");
        builder.HasKey(x => x.NotificationId);
        builder.Property(x => x.Channel).HasMaxLength(32).IsRequired();
        builder.Property(x => x.Recipient).HasMaxLength(255).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(32).IsRequired();
        builder.Property(x => x.FailureReason).HasMaxLength(500);
    }
}