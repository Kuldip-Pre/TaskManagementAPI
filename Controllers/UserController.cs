using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TaskManagementAPI.Data;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    // Constructor to inject the database context
    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // POST method to create a new user
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var existingUser = await _context.Users.AnyAsync(u => u.Username == dto.Name);
            if (existingUser)
                return Conflict("A user with the same username already exists.");

            var user = new User
            {
                Username = dto.Name,
                Password = HashPassword(dto.Password),
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
        }
    }

    // GET method to retrieve a user by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound($"User with ID {id} not found.");

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };

            return Ok(userDto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _context.Users.ToListAsync();
            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role
            }).ToList();

            return Ok(userDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error: {ex.Message}");
        }
    }

    // Helper method to hash the password using SHA-256
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
