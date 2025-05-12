namespace Events
{
    public class UserCreatedEvent
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string GitHubUsername { get; set; }

    }
}
