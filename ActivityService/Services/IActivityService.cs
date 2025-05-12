using ActivityService.DTOs;

namespace ActivityService.Services
{
    public interface IActivityService
    {
        Task<ActivitySummaryDto> GetActivitySummaryAsync(string githubUsername);
    }
}
