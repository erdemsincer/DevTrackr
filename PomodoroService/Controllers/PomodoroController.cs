using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PomodoroService.DTOs;
using PomodoroService.Interfaces;
using System.Security.Claims;

namespace PomodoroService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PomodoroController(IPomodoroService _pomodoroService) : ControllerBase
    {
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpPost]
        public async Task<IActionResult> StartSession(CreatePomodoroDto dto)
        {
            var session = await _pomodoroService.StartSessionAsync(GetUserId(), dto);
            return Ok(session);
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteSession(int id, CompletePomodoroDto dto)
        {
            var success = await _pomodoroService.CompleteSessionAsync(GetUserId(), id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetSessions()
        {
            var sessions = await _pomodoroService.GetSessionsAsync(GetUserId());
            return Ok(sessions);
        }
    }
}
