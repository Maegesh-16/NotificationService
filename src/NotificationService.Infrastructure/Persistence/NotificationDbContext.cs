using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Persistence;

public class NotificationDbContext(DbContextOptions<NotificationDbContext> options) : DbContext(options)
{
    public DbSet<EmailNotification> EmailNotifications => Set<EmailNotification>();
    public DbSet<SmsNotification> SmsNotifications => Set<SmsNotification>();
    public DbSet<PushNotification> PushNotifications => Set<PushNotification>();
    public DbSet<NotificationHistory> NotificationHistory => Set<NotificationHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailNotification>().HasKey(x => x.NotificationId);
        modelBuilder.Entity<SmsNotification>().HasKey(x => x.NotificationId);
        modelBuilder.Entity<PushNotification>().HasKey(x => x.NotificationId);
        modelBuilder.Entity<NotificationHistory>().HasKey(x => x.NotificationId);
    }
}
