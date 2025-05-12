using UserService.DTOs;

namespace UserService.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetProfileAsync(int userId);
        Task<bool> UpdateProfileAsync(int userId, UpdateUserDto dto);
    }
}
