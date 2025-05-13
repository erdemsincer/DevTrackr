using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskService.DTOs;
using TaskService.Interfaces;

namespace TaskService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController(ITaskService _taskService) : ControllerBase
    {
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllAsync(GetUserId());
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(GetUserId(), id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            var created = await _taskService.CreateAsync(GetUserId(), dto);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateTaskDto dto)
        {
            var success = await _taskService.UpdateAsync(GetUserId(), id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _taskService.DeleteAsync(GetUserId(), id);
            return success ? NoContent() : NotFound();
        }
        [HttpGet("{userId}/completed")]
        public async Task<IActionResult> GetCompletedTasks(int userId)
        {
            if (userId != GetUserId())
                return Forbid();

            var completedTasks = await _taskService.GetCompletedTasksAsync(userId);
            return Ok(completedTasks);
        }

    }
}
