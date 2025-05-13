using ActivityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ActivityService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController(IActivityService _activityService) : ControllerBase
    {
        // 🔐 Giriş yapan kullanıcının summary'sini döner
        [Authorize]
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var summary = await _activityService.GetActivitySummaryAsync(int.Parse(userId));
            return Ok(summary);
        }

        // 🔎 Username ile manuel test için de açık kalsın istersen:
        [HttpGet("{githubUsername}")]
        public async Task<IActionResult> GetSummaryByUsername(string githubUsername)
        {
            var summary = await _activityService.GetActivitySummaryAsync(githubUsername);
            return Ok(summary);
        }
    }
}
