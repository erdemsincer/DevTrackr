namespace AiReportService.Entities
{
    public class AiReport
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Auth'dan gelen user ID
        public string Summary { get; set; } = string.Empty; // GPT özeti
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }
}
