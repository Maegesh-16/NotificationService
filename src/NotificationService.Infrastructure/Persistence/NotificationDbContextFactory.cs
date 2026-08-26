using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NotificationService.Infrastructure.Persistence;

public sealed class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
{
    public NotificationDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__NotificationDb");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Set the ConnectionStrings__NotificationDb environment variable before running dotnet ef.");
        }

        var options = new DbContextOptionsBuilder<NotificationDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new NotificationDbContext(options);
    }
}