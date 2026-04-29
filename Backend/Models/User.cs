namespace Backend.Models;
public class User : CouchDocument
{
    public override string Type => "user";
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Member";
    public bool IsApproved { get; set; } = false;
}
