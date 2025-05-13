using TaskService.DTOs;

namespace TaskService.Interfaces
{
    public interface ITaskService
    {
        Task<List<ResultTaskDto>> GetAllAsync(int userId);
        Task<ResultTaskDto?> GetByIdAsync(int userId, int taskId);
        Task<ResultTaskDto> CreateAsync(int userId, CreateTaskDto dto);
        Task<bool> UpdateAsync(int userId, int taskId, UpdateTaskDto dto);
        Task<bool> DeleteAsync(int userId, int taskId);
        Task<List<string>> GetCompletedTasksAsync(int userId);

    }
}
