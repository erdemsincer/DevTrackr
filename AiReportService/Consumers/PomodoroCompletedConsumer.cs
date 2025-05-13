using AiReportService.Services;
using Events;
using MassTransit;

namespace AiReportService.Consumers
{
    public class PomodoroCompletedConsumer : IConsumer<PomodoroCompletedEvent>
    {
        private readonly IAiReportService _aiReportService;

        public PomodoroCompletedConsumer(IAiReportService aiReportService)
        {
            _aiReportService = aiReportService;
        }

        public async Task Consume(ConsumeContext<PomodoroCompletedEvent> context)
        {
            var userId = context.Message.UserId;
            Console.WriteLine($"⏱️ Pomodoro tamamlandı event geldi: userId = {userId}");
            await _aiReportService.GenerateQuickReportAsync(userId);
        }
    }
}
