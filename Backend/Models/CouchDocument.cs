
using System.Text.Json.Serialization;

namespace Backend.Models;

public abstract class CouchDocument
{
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    [JsonPropertyName("_rev")]
    public string? Rev { get; set; }

    [JsonPropertyName("type")]
    public abstract string Type { get; }
}
