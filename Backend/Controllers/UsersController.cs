using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly CouchDbService _couchDb;

    public UsersController(CouchDbService couchDb)
    {
        _couchDb = couchDb;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _couchDb.FindDocumentsAsync<User>(new { selector = new { type = "user" } });
        // Don't send password hashes to frontend
        var safeUsers = users.Select(u => new
        {
            id = u.Id,
            name = u.Name,
            email = u.Email,
            role = u.Role,
            isApproved = u.IsApproved
        });
        return Ok(safeUsers);
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveUser(string id)
    {
        var user = await _couchDb.GetDocumentAsync<User>(id);
        if (user == null) return NotFound();

        user.IsApproved = true;
        await _couchDb.UpdateDocumentAsync(id, user.Rev!, user);
        return Ok(new { message = "User approved" });
    }

    [HttpPut("{id}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateRoleDto dto)
    {
        var user = await _couchDb.GetDocumentAsync<User>(id);
        if (user == null) return NotFound();

        user.Role = dto.Role;
        await _couchDb.UpdateDocumentAsync(id, user.Rev!, user);
        return Ok(new { message = "User role updated" });
    }
}

public class UpdateRoleDto
{
    public string Role { get; set; } = string.Empty;
}
