using AiReportService.External;
using Quartz;

namespace AiReportService.Services
{
    public class WeeklyReportJob : IJob
    {
        private readonly IAiReportService _aiReportService;
        private readonly IUserClient _userClient;

        public WeeklyReportJob(IAiReportService aiReportService, IUserClient userClient)
        {
            _aiReportService = aiReportService;
            _userClient = userClient;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("📆 Haftalık rapor üretimi başladı.");

            var userIds = await _userClient.GetAllUserIdsAsync();
            foreach (var userId in userIds)
            {
                await _aiReportService.GenerateReportAsync(userId);
                Console.WriteLine($"✅ Rapor üretildi: userId={userId}");
            }
        }
    }
}
