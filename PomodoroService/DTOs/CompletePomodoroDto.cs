namespace PomodoroService.DTOs
{
    public class CompletePomodoroDto
    {
        public DateTime EndTime { get; set; } = DateTime.UtcNow;
    }
}
