using PomodoroService.DTOs;

namespace PomodoroService.Interfaces
{
    public interface IPomodoroService
    {
        Task<ResultPomodoroDto?> StartSessionAsync(int userId, CreatePomodoroDto dto);
        Task<bool> CompleteSessionAsync(int userId, int sessionId, CompletePomodoroDto dto);
        Task<List<ResultPomodoroDto>> GetSessionsAsync(int userId);
        Task<List<string>> GetCompletedPomodorosAsync(int userId);

    }
}
