using AiReportService.DTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AiReportService.External
{
    public class ActivityClient : IActivityClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActivityClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ActivitySummaryDto> GetActivitySummaryAsync(int userId)
        {
            AddJwtFromContext();

            var response = await _httpClient.GetAsync("/api/Activity/summary");
            if (!response.IsSuccessStatusCode)
                throw new Exception("ActivityService response error.");

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ActivitySummaryDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new Exception("ActivitySummary deserialization failed.");
        }

        public async Task<List<string>> GetRecentCommitsAsync(int userId)
        {
            AddJwtFromContext();

            var response = await _httpClient.GetAsync("/api/Activity/summary");
            if (!response.IsSuccessStatusCode) return new List<string>();

            var content = await response.Content.ReadAsStringAsync();

            var summary = JsonSerializer.Deserialize<ActivitySummaryDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return new List<string> { $"{summary?.TotalCommits ?? 0} commit" }; // veya daha detaylı örneklerle
        }


        private void AddJwtFromContext()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
