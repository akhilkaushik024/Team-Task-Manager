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
public class ProjectsController : ControllerBase
{
    private readonly CouchDbService _couchDb;

    public ProjectsController(CouchDbService couchDb)
    {
        _couchDb = couchDb;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await _couchDb.FindDocumentsAsync<Project>(new { selector = new { type = "project" } });
        
        if (!User.IsInRole("Admin"))
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userTasks = await _couchDb.FindDocumentsAsync<TaskItem>(new { selector = new { type = "task", assignedToId = userId } });
            var userProjectIds = userTasks.Select(t => t.ProjectId).ToHashSet();
            projects = projects.Where(p => userProjectIds.Contains(p.Id!)).ToList();
        }
        
        return Ok(projects);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedById = User.FindFirstValue(ClaimTypes.NameIdentifier)!
        };

        var id = await _couchDb.CreateDocumentAsync(project);
        return Ok(new { id, message = "Project created successfully" });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProject(string id)
    {
        var project = await _couchDb.GetDocumentAsync<Project>(id);
        if (project == null) return NotFound();

        await _couchDb.DeleteDocumentAsync(id, project.Rev!);

        // Also delete tasks associated with this project
        var tasks = await _couchDb.FindDocumentsAsync<TaskItem>(new { selector = new { type = "task", projectId = id } });
        foreach (var task in tasks)
        {
            await _couchDb.DeleteDocumentAsync(task.Id!, task.Rev!);
        }

        return Ok(new { message = "Project deleted successfully" });
    }
}
