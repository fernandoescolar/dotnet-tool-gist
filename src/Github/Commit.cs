namespace DotnetGist.Github;

public class Commit
{
    [JsonPropertyName("user")]
    public User? User { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("committed_at")]
    public DateTime? CommittedAt { get; set; }

    [JsonPropertyName("change_status")]
    public ChangeStatus? ChangeStatus { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}