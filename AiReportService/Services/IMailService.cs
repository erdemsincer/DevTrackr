using AiReportService.Models;

namespace AiReportService.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest request);
    }
}
