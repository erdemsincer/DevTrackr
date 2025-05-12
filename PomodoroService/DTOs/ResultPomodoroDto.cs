namespace PomodoroService.DTOs
{
    public class ResultPomodoroDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int FocusMinutes { get; set; }
        public int BreakMinutes { get; set; }
        public bool IsCompleted { get; set; }
    }
}
