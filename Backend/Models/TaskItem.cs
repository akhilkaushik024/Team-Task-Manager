namespace Backend.Models;
using System;
public class TaskItem : CouchDocument
{
    public override string Type => "task";
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "TODO";
    public string AssignedToId { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
}
