namespace PomodoroService.DTOs
{
    public class CreatePomodoroDto
    {
        public int FocusMinutes { get; set; } = 25;
        public int BreakMinutes { get; set; } = 5;
    }
}
