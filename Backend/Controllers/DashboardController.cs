using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly CouchDbService _couchDb;

    public DashboardController(CouchDbService couchDb)
    {
        _couchDb = couchDb;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboardStats()
    {
        var selector = new Dictionary<string, object> { { "type", "task" } };
        
        if (!User.IsInRole("Admin"))
        {
            selector["assignedToId"] = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;
        }

        var tasks = await _couchDb.FindDocumentsAsync<TaskItem>(new { selector });
        var now = DateTime.UtcNow;
        
        var stats = new
        {
            TotalTasks = tasks.Count,
            Completed = tasks.Count(t => t.Status == "DONE"),
            InProgress = tasks.Count(t => t.Status == "IN_PROGRESS"),
            Todo = tasks.Count(t => t.Status == "TODO"),
            Overdue = tasks.Count(t => t.Status != "DONE" && t.DueDate < now)
        };

        return Ok(stats);
    }
}
