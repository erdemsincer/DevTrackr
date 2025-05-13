using AiReportService.DTOs;

namespace AiReportService.External
{
    public interface IActivityClient
    {
        Task<ActivitySummaryDto> GetActivitySummaryAsync(int userId);
        Task<List<string>> GetRecentCommitsAsync(int userId);
    }
}
