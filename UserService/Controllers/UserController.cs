using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.DTOs;
using UserService.Interfaces;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService _userService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var result = await _userService.GetProfileAsync(int.Parse(userId));
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UpdateUserDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var updated = await _userService.UpdateProfileAsync(int.Parse(userId), dto);
            return updated ? NoContent() : NotFound();
        }
        [Authorize]
        [HttpGet("github-username")]
        public async Task<IActionResult> GetGithubUsername()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var result = await _userService.GetProfileAsync(int.Parse(userId));
            if (result == null || string.IsNullOrEmpty(result.GitHubUsername)) return NotFound();

            return Ok(result.GitHubUsername); // sadece string
        }
        [HttpGet("ids")]
        public async Task<IActionResult> GetAllUserIds()
        {
            var users = await _userService.GetAllUsersAsync();
            var ids = users.Select(u => u.Id).ToList();
            return Ok(ids);
        }

        [HttpGet("{id}/email")]
        public async Task<IActionResult> GetEmail(int id)
        {
            var email = await _userService.GetUserEmailByIdAsync(id);
            if (email == null)
                return NotFound();

            return Ok(new { email }); // ✅ JSON döndür
        }



    }
}
