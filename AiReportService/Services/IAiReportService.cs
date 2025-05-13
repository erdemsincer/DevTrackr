using AiReportService.Entities;

namespace AiReportService.Services
{
    public interface IAiReportService
    {
        Task<AiReport> GenerateReportAsync(int userId);
        Task GenerateWeeklyReportsAsync();
        Task<string> GenerateQuickReportAsync(int userId);
        Task<List<AiReport>> GetReportsByUserIdAsync(int userId);

    }
}
