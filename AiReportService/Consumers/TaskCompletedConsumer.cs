using AiReportService.Services;
using Events;
using MassTransit;

namespace AiReportService.Consumers
{
    public class TaskCompletedConsumer : IConsumer<TaskCompletedEvent>
    {
        private readonly IAiReportService _aiReportService;

        public TaskCompletedConsumer(IAiReportService aiReportService)
        {
            _aiReportService = aiReportService;
        }

        public async Task Consume(ConsumeContext<TaskCompletedEvent> context)
        {
            var userId = context.Message.UserId;
            Console.WriteLine($"📥 Task tamamlandı event geldi: userId = {userId}");
            await _aiReportService.GenerateQuickReportAsync(userId);
        }
    }
}
