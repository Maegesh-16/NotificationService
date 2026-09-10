using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Notification_ServiceAPI.Authentication;
using Notification_ServiceAPI.Data;
using Notification_ServiceAPI.Middleware;
using Notification_ServiceAPI.Repositories.Implementations;
using Notification_ServiceAPI.Repositories.Interfaces;
using Notification_ServiceAPI.Services.Dispatch;
using Notification_ServiceAPI.Services.Email;
using Notification_ServiceAPI.Services.Implementations;
using Notification_ServiceAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://*:{port}");
}

var notificationServiceConnectionString = builder.Configuration.GetConnectionString("NotificationServiceDb")
    ?? throw new InvalidOperationException("Connection string 'NotificationServiceDb' was not found.");

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(notificationServiceConnectionString));

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationDispatcher, NotificationDispatcher>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(SmtpSettings.SectionName));
builder.Services.AddScoped<IWelcomeEmailSender, SmtpWelcomeEmailSender>();

builder.Services.AddNotificationServiceAuthentication(builder.Configuration);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

var publicBaseUrl = builder.Configuration["AppUrls:PublicBaseUrl"]?.TrimEnd('/');

app.UseForwardedHeaders();

app.UseSwagger(options =>
{
    options.PreSerializeFilters.Add((swagger, httpRequest) =>
    {
        var serverUrl = !string.IsNullOrWhiteSpace(publicBaseUrl)
            ? publicBaseUrl
            : $"{httpRequest.Scheme}://{httpRequest.Host.Value}";

        swagger.Servers =
        [
            new OpenApiServer
            {
                Url = serverUrl
            }
        ];
    });
});

app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception exception)
    {
        app.Logger.LogError(exception, "Notification database migration failed during startup.");
    }
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "Notification Service API" }))
    .AllowAnonymous();

app.MapGet("/", () => Results.Redirect("/swagger"))
    .AllowAnonymous();

app.MapControllers();

app.Run();
