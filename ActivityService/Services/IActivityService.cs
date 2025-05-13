using ActivityService.DTOs;

namespace ActivityService.Services
{
    public interface IActivityService
    {
        Task<ActivitySummaryDto> GetActivitySummaryAsync(string githubUsername); // 🔹 username ile çalışan versiyon
        Task<ActivitySummaryDto> GetActivitySummaryAsync(int userId);           // 🔥 userId ile çalışan versiyon
    }
}
