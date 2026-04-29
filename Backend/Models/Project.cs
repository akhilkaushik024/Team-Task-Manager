namespace Backend.Models;
public class Project : CouchDocument
{
    public override string Type => "project";
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CreatedById { get; set; } = string.Empty;
}
