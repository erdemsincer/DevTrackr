﻿using AiReportService.Data;
using AiReportService.Entities;
using AiReportService.External;
using AiReportService.Models;

using Microsoft.EntityFrameworkCore;

namespace AiReportService.Services
{
    public class AiReportService : IAiReportService
    {
        private readonly AiReportDbContext _context;
        private readonly IActivityClient _activityClient;
        private readonly ITaskClient _taskClient;
        private readonly IPomodoroClient _pomodoroClient;
        private readonly OpenAiService _openAiService;
        private readonly IUserClient _userClient;
        private readonly Services.IEmailService _mailService; // 👈 eklendi


        public AiReportService(
            AiReportDbContext context,
            IActivityClient activityClient,
            ITaskClient taskClient,
            IPomodoroClient pomodoroClient,
            OpenAiService openAiService,
            IUserClient userClient,
            Services.IEmailService mailService) // 👈 eklendi
        {
            _context = context;
            _activityClient = activityClient;
            _taskClient = taskClient;
            _pomodoroClient = pomodoroClient;
            _openAiService = openAiService;
            _userClient = userClient;
            _mailService = mailService;
        }

        public async Task<AiReport> GenerateReportAsync(int userId)
        {
            var commits = await _activityClient.GetRecentCommitsAsync(userId);
            var tasks = await _taskClient.GetCompletedTasksAsync(userId);
            var pomodoros = await _pomodoroClient.GetCompletedPomodorosAsync(userId);
            var summary = await _openAiService.GenerateSummaryAsync(commits, tasks, pomodoros);

            var report = new AiReport
            {
                UserId = userId,
                Summary = summary,
                GeneratedAt = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            // ✉️ Kullanıcının e-postasına gönder
            var email = await _userClient.GetUserEmailAsync(userId);
            if (!string.IsNullOrEmpty(email))
            {
                await _mailService.SendEmailAsync(new MailRequest
                {
                    ToEmail = email,
                    Subject = "Haftalık AI Raporun Hazır!",
                    Body = summary
                });
            }

            return report;
        }

        // ✅ Haftalık toplu rapor
        public async Task GenerateWeeklyReportsAsync()
        {
            var userIds = await _userClient.GetAllUserIdsAsync(); // external HTTP call
            foreach (var userId in userIds)
            {
                await GenerateReportAsync(userId);
            }
        }

        public async Task<string> GenerateQuickReportAsync(int userId)
        {
            var tasks = await _taskClient.GetCompletedTasksAsync(userId);
            var pomodoros = await _pomodoroClient.GetCompletedPomodorosAsync(userId);

            var summary = await _openAiService.GenerateSummaryAsync(new(), tasks, pomodoros);
            Console.WriteLine($"⚡ Kısa analiz üretildi: {summary}");

            return summary;
        }

        public async Task<List<AiReport>> GetReportsByUserIdAsync(int userId)
        {
            return await _context.Reports
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.GeneratedAt)
                .ToListAsync();
        }


    }
}
