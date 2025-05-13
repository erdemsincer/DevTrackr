namespace AiReportService.External
{
    public interface ITaskClient
    {
        Task<List<string>> GetCompletedTasksAsync(int userId);
    }
}
