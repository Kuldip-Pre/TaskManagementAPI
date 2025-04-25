using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor to inject the database context
        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // POST method to create a new task
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _context.Users.FindAsync(dto.UserId);
                if (user == null)
                    return NotFound("User not found.");

                var task = new TaskItem
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    UserId = dto.UserId,
                    User = user
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                var createdTask = await _context.Tasks
                    .Include(t => t.User)
                    .Include(t => t.Comments).ThenInclude(c => c.User)
                    .FirstOrDefaultAsync(t => t.Id == task.Id);

                // If task creation failed, return a 500 error
                if (createdTask == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Task creation failed.");

                return CreatedAtAction(nameof(GetTask), new { id = task.Id }, MapTaskToDto(createdTask));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
            }
        }

        // GET method to retrieve a task by its ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                var task = await _context.Tasks
                    .Include(t => t.User)
                    .Include(t => t.Comments).ThenInclude(c => c.User)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (task == null)
                    return NotFound($"Task with ID {id} not found.");

                return Ok(MapTaskToDto(task));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
            }
        }


        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTasksByUser(int userId)
        {
            try
            {
                var tasks = await _context.Tasks
                    .Where(t => t.UserId == userId)
                    .Include(t => t.User)
                    .Include(t => t.Comments).ThenInclude(c => c.User)
                    .ToListAsync();

                if (tasks == null || !tasks.Any())
                    return NotFound($"No tasks found for user with ID {userId}.");

                return Ok(tasks.Select(MapTaskToDto).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
            }
        }

        // Helper method to map a TaskItem to a TaskDto, including related user and comments
        private TaskDto MapTaskToDto(TaskItem task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), "Task cannot be null");

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                UserId = task.UserId,
                User = task.User == null ? null : new UserDto
                {
                    Id = task.User.Id,
                    Username = task.User.Username
                },
                Comments = task.Comments?.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserId = c.UserId,
                    CreatedAt = c.CreatedAt,
                    User = c.User == null ? null : new UserDto
                    {
                        Id = c.User.Id,
                        Username = c.User.Username
                    }
                }).ToList() ?? new List<CommentDto>()
            };
        }
    }
}
