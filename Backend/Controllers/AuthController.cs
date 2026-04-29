using Backend.Models;
using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly CouchDbService _couchDb;
    private readonly IConfiguration _config;

    public AuthController(CouchDbService couchDb, IConfiguration config)
    {
        _couchDb = couchDb;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var existing = await _couchDb.FindDocumentsAsync<User>(new { selector = new { type = "user", email = dto.Email } });
        if (existing.Any()) return BadRequest(new { message = "Email already exists" });

        var allUsers = await _couchDb.FindDocumentsAsync<User>(new { selector = new { type = "user" } });
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = allUsers.Any() ? "Member" : "Admin",
            IsApproved = !allUsers.Any() // First user is auto-approved
        };

        var id = await _couchDb.CreateDocumentAsync(user);
        return Ok(new { message = "User registered successfully", id });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var users = await _couchDb.FindDocumentsAsync<User>(new { selector = new { type = "user", email = dto.Email } });
        var user = users.FirstOrDefault();
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid credentials" });

        if (!user.IsApproved)
            return Unauthorized(new { message = "Account pending admin approval" });

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["JwtSettings:Secret"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("name", user.Name)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new
        {
            token = tokenHandler.WriteToken(token),
            user = new { id = user.Id, name = user.Name, email = user.Email, role = user.Role }
        });
    }
}
