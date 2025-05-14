using System.Text.Json;

namespace AiReportService.External
{
    public class UserClient : IUserClient
    {
        private readonly HttpClient _httpClient;

        public UserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<int>> GetAllUserIdsAsync()
        {
            var response = await _httpClient.GetAsync("/api/User/ids");
            if (!response.IsSuccessStatusCode) return new();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<int>>(content) ?? new();
        }
        public async Task<string?> GetUserEmailAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"/api/User/{userId}/email");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);
            return json.RootElement.GetProperty("email").GetString();
        }

    }
}
