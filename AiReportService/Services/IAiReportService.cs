using AiReportService.Entities;

namespace AiReportService.Services
{
    public interface IAiReportService
    {
        Task<AiReport> GenerateReportAsync(int userId);
    }
}
