using System.Net.Http.Headers;
using System.Text.Json;

namespace AiReportService.External
{
    public class PomodoroClient : IPomodoroClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PomodoroClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<string>> GetCompletedPomodorosAsync(int userId)
        {
            AddJwtFromContext();

            var response = await _httpClient.GetAsync($"/api/Pomodoro/{userId}/completed");
            if (!response.IsSuccessStatusCode) return new List<string>();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<string>();
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