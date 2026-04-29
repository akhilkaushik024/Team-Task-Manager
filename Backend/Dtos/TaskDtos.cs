namespace Backend.Dtos;
using System;
public class CreateTaskDto { public string Title { get; set; } = string.Empty; public string Description { get; set; } = string.Empty; public string AssignedToId { get; set; } = string.Empty; public string ProjectId { get; set; } = string.Empty; public DateTime DueDate { get; set; } }
public class UpdateTaskStatusDto { public string Status { get; set; } = string.Empty; }
