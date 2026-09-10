using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Notification_ServiceAPI.Services.Email;

public class SmtpWelcomeEmailSender(IOptions<SmtpSettings> smtpSettings) : IWelcomeEmailSender
{
    public async Task SendAsync(string recipientEmail, string recipientName, CancellationToken cancellationToken = default)
    {
        var settings = smtpSettings.Value;
        ValidateSettings(settings);

        using var message = new MailMessage
        {
            From = new MailAddress(settings.FromAddress, settings.FromName),
            Subject = "Welcome",
            Body = $"Hello {recipientName},\n\nWelcome to our service. Your account has successfully signed in.\n\nThank you.",
            IsBodyHtml = false
        };
        message.To.Add(recipientEmail);

        using var client = new SmtpClient(settings.Host, settings.Port)
        {
            EnableSsl = settings.EnableSsl,
            Credentials = new NetworkCredential(settings.Username, settings.Password)
        };

        await client.SendMailAsync(message, cancellationToken);
    }

    private static void ValidateSettings(SmtpSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.Host) ||
            string.IsNullOrWhiteSpace(settings.Username) ||
            string.IsNullOrWhiteSpace(settings.Password) ||
            string.IsNullOrWhiteSpace(settings.FromAddress))
        {
            throw new InvalidOperationException("SMTP settings are incomplete. Configure Smtp__Host, Smtp__Username, Smtp__Password, and Smtp__FromAddress.");
        }
    }
}