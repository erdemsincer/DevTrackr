using Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using TaskService.Data;
using TaskService.DTOs;
using TaskService.Interfaces;
using TaskService.Models;

namespace TaskService.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskDbContext _context;

        private readonly IPublishEndpoint _publishEndpoint;

        public TaskService(TaskDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }


        public async Task<List<ResultTaskDto>> GetAllAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .Select(t => new ResultTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    CreatedAt = t.CreatedAt,
                    CompletedAt = t.CompletedAt
                }).ToListAsync();
        }

        public async Task<ResultTaskDto?> GetByIdAsync(int userId, int taskId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null) return null;

            return new ResultTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                CompletedAt = task.CompletedAt
            };
        }

        public async Task<ResultTaskDto> CreateAsync(int userId, CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new ResultTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int userId, int taskId, UpdateTaskDto dto)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
            if (task == null) return false;

            task.Title = dto.Title;
            task.Description = dto.Description;

            // Önceki tamamlanma durumu false → şimdi true olduysa event fırlat
            var wasCompleted = task.IsCompleted;
            task.IsCompleted = dto.IsCompleted;
            task.CompletedAt = dto.IsCompleted ? DateTime.UtcNow : null;

            await _context.SaveChangesAsync();

            if (!wasCompleted && dto.IsCompleted)
            {
                await _publishEndpoint.Publish(new TaskCompletedEvent
                {
                    UserId = userId,
                    TaskTitle = task.Title
                });
            }

            return true;
        }


        public async Task<bool> DeleteAsync(int userId, int taskId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetCompletedTasksAsync(int userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId && t.IsCompleted)
                .OrderByDescending(t => t.CompletedAt)
                .Select(t => t.Title)
                .ToListAsync();
        }

    }
}
