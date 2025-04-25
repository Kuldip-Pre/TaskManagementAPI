using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;

[Authorize]  // Ensures that the user is authenticated
[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public CommentsController(AppDbContext context)
    {
        _context = context;
    }

    // POST method to create a new comment
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto commentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // Optionally validate that the task and user exist in the database
            var task = await _context.Tasks.FindAsync(commentDto.TaskItemId);
            var user = await _context.Users.FindAsync(commentDto.UserId);

            if (task == null)
                return NotFound($"Task with ID {commentDto.TaskItemId} not found.");

            if (user == null)
                return NotFound($"User with ID {commentDto.UserId} not found.");

            // Create a new comment based on the DTO
            var comment = new TaskComment
            {
                Content = commentDto.Content,
                TaskItemId = commentDto.TaskItemId,
                UserId = commentDto.UserId,
                CreatedAt = DateTime.UtcNow
            };

          
            _context.TaskComments.Add(comment);
            await _context.SaveChangesAsync();

            // Return the created comment with a 201 Created response
            var createdCommentDto = new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                TaskItemId = comment.TaskItemId,
                UserId = comment.UserId,
                CreatedAt = comment.CreatedAt,
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role
                }
            };

            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, createdCommentDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
        }
    }

    // GET method to retrieve all comments for a specific task
    [HttpGet("task/{taskId}")]
    public async Task<IActionResult> GetCommentsByTask(int taskId)
    {
        try
        {
           
            var comments = await _context.TaskComments
                .Where(c => c.TaskItemId == taskId)
                .Include(c => c.User)
                .ToListAsync();

            // Map comments to DTOs
            var commentDtos = comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                TaskItemId = c.TaskItemId,
                UserId = c.UserId,
                CreatedAt = c.CreatedAt,
                User = new UserDto
                {
                    Id = c.User.Id,
                    Username = c.User.Username,
                    Role = c.User.Role
                }
            }).ToList();

            return Ok(commentDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
        }
    }

    // GET method to retrieve a specific comment by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById(int id)
    {
        try
        {
           
            var comment = await _context.TaskComments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
                return NotFound();

            // Map comment to DTO
            var commentDto = new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                TaskItemId = comment.TaskItemId,
                UserId = comment.UserId,
                CreatedAt = comment.CreatedAt,
                User = new UserDto
                {
                    Id = comment.User.Id,
                    Username = comment.User.Username,
                    Role = comment.User.Role
                }
            };

            return Ok(commentDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
        }
    }
}
