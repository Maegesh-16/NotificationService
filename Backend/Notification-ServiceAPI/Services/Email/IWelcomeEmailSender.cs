namespace Notification_ServiceAPI.Services.Email;

public interface IWelcomeEmailSender
{
    Task SendAsync(string recipientEmail, string recipientName, CancellationToken cancellationToken = default);
}