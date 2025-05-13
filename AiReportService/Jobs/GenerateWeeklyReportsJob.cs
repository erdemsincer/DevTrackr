using AiReportService.External;
using AiReportService.Services;
using Quartz;

namespace AiReportService.Jobs
{
    public class GenerateWeeklyReportsJob : IJob
    {
        private readonly IUserClient _userClient;
        private readonly IAiReportService _aiReportService;

        public GenerateWeeklyReportsJob(IUserClient userClient, IAiReportService aiReportService)
        {
            _userClient = userClient;
            _aiReportService = aiReportService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var userIds = await _userClient.GetAllUserIdsAsync();
            foreach (var userId in userIds)
            {
                await _aiReportService.GenerateReportAsync(userId);
            }
        }
    }
}
