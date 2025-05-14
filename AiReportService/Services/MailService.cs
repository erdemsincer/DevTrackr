using AiReportService.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AiReportService.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(MailRequest request)
        {
            var from = Environment.GetEnvironmentVariable("MAIL_FROM")!;
            var smtpHost = Environment.GetEnvironmentVariable("MAIL_SMTP_HOST")!;
            var smtpPort = int.Parse(Environment.GetEnvironmentVariable("MAIL_SMTP_PORT")!);
            var smtpUser = Environment.GetEnvironmentVariable("MAIL_SMTP_USER")!;
            var smtpPass = Environment.GetEnvironmentVariable("MAIL_SMTP_PASS")!;

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = request.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = request.Body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(smtpUser, smtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
