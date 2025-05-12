namespace ActivityService.DTOs
{
    public class GitHubRepoDto
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public int Stargazers_Count { get; set; }
        public DateTime Pushed_At { get; set; }
    }
}
