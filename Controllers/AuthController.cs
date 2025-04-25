using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context; // Database context for accessing user data
    private readonly IConfiguration _config;

    // Constructor that takes in the database context and configuration settings
    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // Login endpoint that accepts a POST request with the username and password
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
        if (user == null)
        {
            return Unauthorized("Invalid credentials");
        }

        // Create the claims for the JWT token (e.g., username and role)
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);


        // Create the JWT token with the claims and signing credentials
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
           expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
