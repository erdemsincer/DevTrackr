using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTOs;
using UserService.Interfaces;
using UserService.Models;

namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _context;

        public UserService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> GetProfileAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                FullName = user.FullName,
                Email = user.Email,
                GitHubUsername = user.GitHubUsername,
                Bio = user.Bio,
                Theme = user.Theme
            };
        }

        public async Task<bool> UpdateProfileAsync(int userId, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.FullName = dto.FullName;
            user.GitHubUsername = dto.GitHubUsername;
            user.Bio = dto.Bio;
            user.Theme = dto.Theme;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

    }
}
