using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;

namespace Infrastructure.Mailer;

public class SmtpEmailSender : IEmailSender
{
    private readonly string SMTP_HOST = "smtp.gmail.com";
    private readonly int SMTP_PORT = 587;
    private readonly string SMTP_USERNAME = "connecting0316@gmail.com";
    private readonly string SMTP_PASSWORD = "vevr ckcz lory lezo";
    private readonly string SMTP_FROM = "no-reply@pathlock.com";
    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody, CancellationToken ct = default)
    {
        var host = SMTP_HOST;
        var port = SMTP_PORT;
        var user = SMTP_USERNAME;
        var pass = SMTP_PASSWORD;
        var from = SMTP_FROM;

        var msg = new MimeMessage();
        msg.From.Add(MailboxAddress.Parse(from));
        msg.To.Add(MailboxAddress.Parse(toEmail));
        msg.Subject = subject;
        msg.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();

        await client.ConnectAsync(host, port, SecureSocketOptions.StartTls, ct);

        if (!string.IsNullOrEmpty(user))
            await client.AuthenticateAsync(user, pass, ct);

        await client.SendAsync(msg, ct);
        await client.DisconnectAsync(true, ct);
    }
}
