using AiReportService.Services;
using Events;
using MassTransit;

namespace AiReportService.Consumers
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
    {
        private readonly IAiReportService _aiReportService;

        public UserRegisteredConsumer(IAiReportService aiReportService)
        {
            _aiReportService = aiReportService;
        }

        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var userId = context.Message.UserId;
            Console.WriteLine($"📩 Yeni kullanıcı kaydı alındı: userId = {userId}");
            await _aiReportService.GenerateReportAsync(userId);
        }
    }
}
