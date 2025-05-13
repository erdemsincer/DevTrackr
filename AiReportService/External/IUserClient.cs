namespace AiReportService.External
{
    public interface IUserClient
    {
        Task<List<int>> GetAllUserIdsAsync();
    }
}
