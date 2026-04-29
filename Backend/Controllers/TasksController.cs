using Backend.Models;
using Backend.Dtos;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly CouchDbService _couchDb;

    public TasksController(CouchDbService couchDb)
    {
        _couchDb = couchDb;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] string? projectId, [FromQuery] string? assignedToId)
    {
        var selector = new Dictionary<string, object> { { "type", "task" } };
        if (!string.IsNullOrEmpty(projectId)) selector["projectId"] = projectId;
        
        if (!User.IsInRole("Admin"))
        {
            selector["assignedToId"] = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;
        }
        else if (!string.IsNullOrEmpty(assignedToId)) 
        {
            selector["assignedToId"] = assignedToId;
        }

        var tasks = await _couchDb.FindDocumentsAsync<TaskItem>(new { selector });
        return Ok(tasks);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            AssignedToId = dto.AssignedToId,
            ProjectId = dto.ProjectId,
            DueDate = dto.DueDate,
            Status = "TODO"
        };
        var id = await _couchDb.CreateDocumentAsync(task);
        return Ok(new { id, message = "Task created successfully" });
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateTaskStatusDto dto)
    {
        var task = await _couchDb.GetDocumentAsync<TaskItem>(id);
        if (task == null) return NotFound();

        task.Status = dto.Status;
        await _couchDb.UpdateDocumentAsync(task.Id!, task.Rev!, task);
        return Ok(new { message = "Status updated" });
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTask(string id)
    {
        var task = await _couchDb.GetDocumentAsync<TaskItem>(id);
        if (task == null) return NotFound();
        await _couchDb.DeleteDocumentAsync(task.Id!, task.Rev!);
        return Ok(new { message = "Task deleted" });
    }
}
