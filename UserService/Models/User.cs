namespace UserService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string GitHubUsername { get; set; }
        public string Bio { get; set; }
        public string Theme { get; set; } = "light"; // light / dark
    }
}
