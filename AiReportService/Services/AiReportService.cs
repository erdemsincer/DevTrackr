using AiReportService.Data;
using AiReportService.Entities;
using AiReportService.External;

namespace AiReportService.Services
{
    public class AiReportService : IAiReportService
    {
        private readonly AiReportDbContext _context;
        private readonly IActivityClient _activityClient;
        private readonly ITaskClient _taskClient;
        private readonly IPomodoroClient _pomodoroClient;
        private readonly OpenAiService _openAiService;

        public AiReportService(
            AiReportDbContext context,
            IActivityClient activityClient,
            ITaskClient taskClient,
            IPomodoroClient pomodoroClient,
            OpenAiService openAiService)
        {
            _context = context;
            _activityClient = activityClient;
            _taskClient = taskClient;
            _pomodoroClient = pomodoroClient;
            _openAiService = openAiService;
        }

        public async Task<AiReport> GenerateReportAsync(int userId)
        {
            // 1. Verileri çek
            var commits = await _activityClient.GetRecentCommitsAsync(userId);
            var tasks = await _taskClient.GetCompletedTasksAsync(userId);
            var pomodoros = await _pomodoroClient.GetCompletedPomodorosAsync(userId);

            // 2. OpenAI'den özet al
            var summary = await _openAiService.GenerateSummaryAsync(commits, tasks, pomodoros);

            // 3. Veritabanına kaydet
            var report = new AiReport
            {
                UserId = userId,
                Summary = summary,
                GeneratedAt = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return report;
        }
    }
}
