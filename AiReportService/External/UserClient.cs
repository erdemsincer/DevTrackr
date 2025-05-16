using System.Net.Http.Headers;
using System.Text.Json;

namespace AiReportService.External
{
    public class UserClient : IUserClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddJwtFromContext()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<int>> GetAllUserIdsAsync()
        {
            AddJwtFromContext();
            var response = await _httpClient.GetAsync("/api/User/ids");
            if (!response.IsSuccessStatusCode) return new();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<int>>(content) ?? new();
        }

        public async Task<string?> GetUserEmailAsync(int userId)
        {
            AddJwtFromContext();
            var response = await _httpClient.GetAsync($"/api/User/{userId}/email");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);
            return json.RootElement.GetProperty("email").GetString();
        }
    }
}
