using Microsoft.EntityFrameworkCore;
using PomodoroService.Data;
using PomodoroService.DTOs;
using PomodoroService.Interfaces;
using PomodoroService.Models;
using Events;
using MassTransit;

namespace PomodoroService.Services
{
    public class PomodoroService : IPomodoroService
    {
        private readonly PomodoroDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public PomodoroService(PomodoroDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

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

            // 🔥 PomodoroCompletedEvent fırlat
            await _publishEndpoint.Publish(new PomodoroCompletedEvent
            {
                UserId = userId
            });

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

        public async Task<List<string>> GetCompletedPomodorosAsync(int userId)
        {
            return await _context.Sessions
                .Where(x => x.UserId == userId && x.IsCompleted)
                .OrderByDescending(x => x.EndTime)
                .Select(x => $"Focused {x.FocusMinutes} min, Break {x.BreakMinutes} min on {x.StartTime:yyyy-MM-dd HH:mm}")
                .ToListAsync();
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
