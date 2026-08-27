using Microsoft.EntityFrameworkCore;
using Notification_ServiceAPI.Models.Notifications;

namespace Notification_ServiceAPI.Data;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options)
    {
    }

    public DbSet<EmailNotification> EmailNotifications => Set<EmailNotification>();

    public DbSet<SmsNotification> SmsNotifications => Set<SmsNotification>();

    public DbSet<PushNotification> PushNotifications => Set<PushNotification>();

    public DbSet<NotificationHistory> NotificationHistories => Set<NotificationHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
    }
}