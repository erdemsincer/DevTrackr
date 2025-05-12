namespace AuthService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string GitHubUsername { get; set; }  // GitHub OAuth sonrası gelecek
        public string Email { get; set; }
        public string PasswordHash { get; set; }    // Normal login için (opsiyonel)
        public string FullName { get; set; }

        public string Role { get; set; } = "User";  // İleride admin vs ayrımı için
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
