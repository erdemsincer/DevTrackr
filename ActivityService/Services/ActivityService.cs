using ActivityService.DTOs;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ActivityService.Services
{
    public class ActivityService : IActivityService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ActivityService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            var githubToken = _config["GitHub:Token"];
            Console.WriteLine("📦 Token Loaded: " + githubToken);

            if (!string.IsNullOrEmpty(githubToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("token", githubToken);
            }

            _httpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("DevTrackr", "1.0"));
        }


        public async Task<ActivitySummaryDto> GetActivitySummaryAsync(string githubUsername)
        {
            var repos = await GetReposAsync(githubUsername);
            int totalCommits = 0;

            foreach (var repo in repos)
            {
                var url = $"https://api.github.com/repos/{githubUsername}/{repo.Name}/contributors";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode) continue;

                var content = await response.Content.ReadAsStringAsync();
                var contributors = JsonSerializer.Deserialize<List<ContributorDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var user = contributors?.FirstOrDefault(c => c.Login == githubUsername);
                if (user != null)
                    totalCommits += user.Contributions;
            }

            var mostUsedLang = repos
                .Where(r => !string.IsNullOrEmpty(r.Language))
                .GroupBy(r => r.Language)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key ?? "Yok";

            var mostStarred = repos
                .OrderByDescending(r => r.Stargazers_Count)
                .FirstOrDefault();

            var lastPushed = repos
                .OrderByDescending(r => r.Pushed_At)
                .FirstOrDefault();

            return new ActivitySummaryDto
            {
                TotalCommits = totalCommits,
                RepoCount = repos.Count,
                MostUsedLanguage = mostUsedLang,
                MostStarredRepo = mostStarred?.Name ?? "-",
                MostStars = mostStarred?.Stargazers_Count ?? 0,
                LastPushedRepo = lastPushed?.Name ?? "-",
                LastPushDate = lastPushed?.Pushed_At ?? DateTime.MinValue
            };
        }

        private async Task<List<GitHubRepoDto>> GetReposAsync(string githubUsername)
        {
            var url = $"https://api.github.com/users/{githubUsername}/repos";
            var response = await _httpClient.GetAsync(url);

            Console.WriteLine($"🔎 Fetching repos for: {githubUsername}, status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return new List<GitHubRepoDto>();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<GitHubRepoDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<GitHubRepoDto>();
        }

    }
}
