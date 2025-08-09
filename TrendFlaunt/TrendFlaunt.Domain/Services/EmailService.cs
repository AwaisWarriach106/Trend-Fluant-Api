using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using TrendFlaunt.Data.Settings;
using TrendFlaunt.Domain.Common;
using TrendFlaunt.Domain.Interfaces;
using TrendFlaunt.Domain.ResponseModel;

namespace TrendFlaunt.Domain.Services;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfig;
    private readonly ILogger<EmailService> _logger;
    public EmailService(EmailConfiguration emailConfig, ILogger<EmailService> logger)
    {
        _emailConfig = emailConfig;
        _logger = logger;
    }

    public void SendEmail(MessageResponse message)
    {
        var emailMessage = CreateEmailMessage(message);
        Send(emailMessage);
    }

    private MimeMessage CreateEmailMessage(MessageResponse message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Trend Flaunt Support Team", _emailConfig.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

        return emailMessage;
    }

    private void Send(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) =>
            {
                if (_emailConfig.SmtpServer.Contains("smtp.gmail.com"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };

            client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

            client.Send(mailMessage);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error sending email: {ex}");
            _logger.LogError(ex, "Error sending email message", ErrorCode.Error);
            throw new InvalidOperationException("An error occurred while sending the email.", ex);
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }
    }
}
