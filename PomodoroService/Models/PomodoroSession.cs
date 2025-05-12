namespace PomodoroService.Models
{
    public class PomodoroSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime? EndTime { get; set; }

        public int FocusMinutes { get; set; }
        public int BreakMinutes { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
