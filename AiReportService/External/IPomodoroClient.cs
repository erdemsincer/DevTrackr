namespace AiReportService.External
{
    public interface IPomodoroClient
    {
        Task<List<string>> GetCompletedPomodorosAsync(int userId);
    }
}
