using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Contracts;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("NotificationDb")
            ?? throw new InvalidOperationException("Connection string 'NotificationDb' was not found.");

        services.AddDbContext<NotificationDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<INotificationDispatchService, NotificationDispatchService>();

        return services;
    }
}
