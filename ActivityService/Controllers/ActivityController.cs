using ActivityService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ActivityService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController(IActivityService _activityService) : ControllerBase
    {
        [HttpGet("{githubUsername}")]
        public async Task<IActionResult> GetSummary(string githubUsername)
        {
            var summary = await _activityService.GetActivitySummaryAsync(githubUsername);
            return Ok(summary);
        }
    }
}
