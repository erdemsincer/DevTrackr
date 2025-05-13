namespace Events
{
    public class TaskCompletedEvent
    {
        public int UserId { get; set; }
        public string TaskTitle { get; set; }
    }
}
