namespace AiReportService.DTOs
{
    public class ActivitySummaryDto
    {
        public int TotalCommits { get; set; }
        public int RepoCount { get; set; }
        public string MostUsedLanguage { get; set; }
        public string MostStarredRepo { get; set; }
        public int MostStars { get; set; }
        public string LastPushedRepo { get; set; }
        public DateTime LastPushDate { get; set; }
    }
}
