using AiReportService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AiReportService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AiReportController : ControllerBase
    {
        private readonly IAiReportService _aiReportService;

        public AiReportController(IAiReportService aiReportService)
        {
            _aiReportService = aiReportService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            var userId = int.Parse(userIdStr);

            var report = await _aiReportService.GenerateReportAsync(userId);
            return Ok(report);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetReportsByUser(int userId)
        {
            var reports = await _aiReportService.GetReportsByUserIdAsync(userId);
            return Ok(reports);
        }
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyReports()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var reports = await _aiReportService.GetReportsByUserIdAsync(userId);
            return Ok(reports);
        }



    }
}
