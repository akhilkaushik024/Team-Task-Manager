using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Services;

public class CouchDbService
{
    private readonly HttpClient _httpClient;
    private readonly string _databaseName;
    private readonly ILogger<CouchDbService> _logger;

    public CouchDbService(IConfiguration config, ILogger<CouchDbService> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        
        var url = config["CouchDB:Url"] ?? "http://localhost:5984";
        var user = config["CouchDB:Username"] ?? "admin";
        var pass = config["CouchDB:Password"] ?? "password";
        _databaseName = config["CouchDB:DatabaseName"] ?? "teamtaskmanager";

        var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{pass}"));
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", auth);
        _httpClient.BaseAddress = new Uri(url);
    }

    public async Task EnsureDatabaseExistsAsync()
    {
        var response = await _httpClient.GetAsync($"/{_databaseName}");
        if (!response.IsSuccessStatusCode)
        {
            await _httpClient.PutAsync($"/{_databaseName}", null);
            _logger.LogInformation($"Created CouchDB database: {_databaseName}");
        }
    }

    public async Task<T?> GetDocumentAsync<T>(string id)
    {
        var response = await _httpClient.GetAsync($"/{_databaseName}/{id}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        return default;
    }

    public async Task<string?> CreateDocumentAsync<T>(T document)
    {
        var json = JsonSerializer.Serialize(document, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"/{_databaseName}", content);
        
        if (response.IsSuccessStatusCode)
        {
            var resStr = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(resStr);
            return doc.RootElement.GetProperty("id").GetString();
        }
        else
        {
            var errStr = await response.Content.ReadAsStringAsync();
            _logger.LogError($"CouchDB Create Error: {response.StatusCode} - {errStr} \n Payload: {json}");
        }
        return null;
    }

    public async Task<bool> UpdateDocumentAsync<T>(string id, string rev, T document)
    {
        var json = JsonSerializer.Serialize(document, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"/{_databaseName}/{id}?rev={rev}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteDocumentAsync(string id, string rev)
    {
        var response = await _httpClient.DeleteAsync($"/{_databaseName}/{id}?rev={rev}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<T>> FindDocumentsAsync<T>(object query)
    {
        var json = JsonSerializer.Serialize(query, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"/{_databaseName}/_find", content);
        
        var list = new List<T>();
        var resStr = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            using var doc = JsonDocument.Parse(resStr);
            if (doc.RootElement.TryGetProperty("docs", out var docsElement))
            {
                foreach (var el in docsElement.EnumerateArray())
                {
                    var item = JsonSerializer.Deserialize<T>(el.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (item != null) list.Add(item);
                }
            }
        }
        else
        {
            _logger.LogError($"CouchDB Find Error: {response.StatusCode} - {resStr} \n Payload: {json}");
        }
        return list;
    }
}
