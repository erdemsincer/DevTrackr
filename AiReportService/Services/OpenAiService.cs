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
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("OpenAI API key is missing.");

            Console.WriteLine("🔑 OpenAI API Key Loaded.");

            _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<string> GenerateSummaryAsync(List<string> commits, List<string> tasks, List<string> pomodoros)
        {
            var systemPrompt = _config["OpenAI:SystemPrompt"] ?? """
Sen deneyimli bir yazılım geliştirme koçusun. Yazılımcının haftalık aktivitelerini analiz eder, özlü bir özet çıkarır ve gelişim için somut öneriler sunarsın.
""";

            var userPrompt = $"""
Bir yazılımcının bu haftaki verileri:

🔹 Commit Sayısı: {commits.Count} ({string.Join(", ", commits.Take(3))})
🔹 Tamamlanan Görev Sayısı: {tasks.Count} ({string.Join(", ", tasks.Take(3))})
🔹 Yapılan Pomodoro Sayısı: {pomodoros.Count}

Lütfen aşağıdaki çıktıyı oluştur:
1. Bu haftanın kısa ve öz bir özeti (en fazla 3 cümle).
2. Geliştiriciye yönelik 2-3 somut öneri.

Cevap tonu: pozitif, yapıcı, motive edici.
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
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("chat/completions", content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"❌ OpenAI API error {response.StatusCode}: {result}");
                    return $"GPT çağrısı başarısız oldu: {response.StatusCode}";
                }

                using var doc = JsonDocument.Parse(result);
                var reply = doc.RootElement
                               .GetProperty("choices")[0]
                               .GetProperty("message")
                               .GetProperty("content")
                               .GetString();

                return reply ?? "GPT boş cevap verdi.";
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Hata oluştu: " + ex.Message);
                return "GPT cevabı çözümlenemedi.";
            }
        }
    }
}
