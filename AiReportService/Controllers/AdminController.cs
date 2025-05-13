using AiReportService.External;
using AiReportService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AiReportService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserClient _userClient;
        private readonly IAiReportService _aiReportService;

        public AdminController(IUserClient userClient, IAiReportService aiReportService)
        {
            _userClient = userClient;
            _aiReportService = aiReportService;
        }

        [HttpPost("generate-weekly-reports")]
        public async Task<IActionResult> GenerateAllReports()
        {
            var userIds = await _userClient.GetAllUserIdsAsync();

            foreach (var userId in userIds)
            {
                await _aiReportService.GenerateReportAsync(userId);
            }

            return Ok($"{userIds.Count} kullanıcı için haftalık rapor üretildi.");
        }
    }
}
