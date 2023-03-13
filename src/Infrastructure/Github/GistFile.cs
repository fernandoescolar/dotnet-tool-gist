namespace DotnetGist.Infrastructure.Github;

public class GistFile
{
    [JsonPropertyName("filename")]
    public string? Filename { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("raw_url")]
    public string? RawUrl { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("truncated")]
    public bool Truncated { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }
}
