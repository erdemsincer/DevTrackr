using AiReportService.Settings;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using AiReportService.Models;
using MimeKit;
using MailKit.Security;

namespace AiReportService.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(MailRequest request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["MailSettings:SenderEmail"]));
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = request.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = request.Body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient(); // ✅ doğru: MailKit

            await smtp.ConnectAsync(_config["MailSettings:SmtpHost"], int.Parse(_config["MailSettings:SmtpPort"]!), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["MailSettings:Username"], _config["MailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
