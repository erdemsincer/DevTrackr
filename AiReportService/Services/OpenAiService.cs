using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace AiReportService.Services
{
    public class OpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public OpenAiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            var apiKey = _config["OPENAI_API_KEY"];
            Console.WriteLine("🔑 OpenAI API Key Loaded: " + apiKey); // 🔍 Log ekle

            _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<string> GenerateSummaryAsync(List<string> commits, List<string> tasks, List<string> pomodoros)
        {
            var systemPrompt = _config["OpenAI:SystemPrompt"] ?? "Sen bir yazılım koçusun. Haftalık verileri özetle.";

            var userPrompt = $"""
    İşte bu haftanın verileri:
    - Commitler: {commits.Count} adet ({string.Join(", ", commits.Take(3))})
    - Görevler: {tasks.Count} adet ({string.Join(", ", tasks.Take(3))})
    - Pomodorolar: {pomodoros.Count} adet

    Kısa ama etkili bir özet + öneri ver.
    """;

            var requestBody = new
            {
                model = _config["OpenAI:Model"] ?? "gpt-4",
                messages = new[]
                {
            new { role = "system", content = systemPrompt },
            new { role = "user", content = userPrompt }
        }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var response = await _httpClient.PostAsync("chat/completions", new StringContent(json, Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("❌ OpenAI response error:");
                Console.WriteLine(result);
                return $"GPT çağrısı başarısız oldu: {response.StatusCode}";
            }

            try
            {
                using var doc = JsonDocument.Parse(result);
                if (doc.RootElement.TryGetProperty("choices", out var choices))
                {
                    var reply = choices[0].GetProperty("message").GetProperty("content").GetString();
                    return reply ?? "GPT boş cevap verdi.";
                }
                else
                {
                    return "❌ GPT 'choices' içermeyen bir cevap döndü.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ JSON parse hatası: " + ex.Message);
                return "GPT cevabı çözümlenemedi.";
            }
        }

    }
}
