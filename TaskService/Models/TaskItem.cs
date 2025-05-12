namespace TaskService.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }             // Görev kimin → Auth token'dan gelecek
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
    }
}
