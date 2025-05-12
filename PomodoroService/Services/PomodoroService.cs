using Microsoft.EntityFrameworkCore;
using PomodoroService.Data;
using PomodoroService.DTOs;
using PomodoroService.Interfaces;
using PomodoroService.Models;

namespace PomodoroService.Services
{
    public class PomodoroService(PomodoroDbContext _context) : IPomodoroService
    {
        public async Task<ResultPomodoroDto?> StartSessionAsync(int userId, CreatePomodoroDto dto)
        {
            var session = new PomodoroSession
            {
                UserId = userId,
                FocusMinutes = dto.FocusMinutes,
                BreakMinutes = dto.BreakMinutes,
                StartTime = DateTime.UtcNow
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return MapToDto(session);
        }

        public async Task<bool> CompleteSessionAsync(int userId, int sessionId, CompletePomodoroDto dto)
        {
            var session = await _context.Sessions
                .FirstOrDefaultAsync(x => x.Id == sessionId && x.UserId == userId);

            if (session == null || session.IsCompleted) return false;

            session.EndTime = dto.EndTime;
            session.IsCompleted = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ResultPomodoroDto>> GetSessionsAsync(int userId)
        {
            var sessions = await _context.Sessions
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.StartTime)
                .ToListAsync();

            return sessions.Select(MapToDto).ToList();
        }

        private static ResultPomodoroDto MapToDto(PomodoroSession session)
        {
            return new ResultPomodoroDto
            {
                Id = session.Id,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                FocusMinutes = session.FocusMinutes,
                BreakMinutes = session.BreakMinutes,
                IsCompleted = session.IsCompleted
            };
        }
    }
}
