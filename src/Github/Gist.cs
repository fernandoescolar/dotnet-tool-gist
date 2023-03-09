namespace DotnetGist.Github;

public class Gist
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("forks_url")]
    public string? ForksUrl { get; set; }

    [JsonPropertyName("commits_url")]
    public string? CommitsUrl { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("node_id")]
    public string? NodeId { get; set; }

    [JsonPropertyName("git_pull_url")]
    public string? GitPullUrl { get; set; }

    [JsonPropertyName("git_push_url")]
    public string? GitPushUrl { get; set; }

    [JsonPropertyName("html_url")]
    public string? HtmlUrl { get; set; }

    [JsonPropertyName("files")]
    public Dictionary<string, GistFile> Files { get; set; } = new Dictionary<string, GistFile>();

    [JsonPropertyName("public")]
    public bool Public { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("comments")]
    public int Comments { get; set; }

    [JsonPropertyName("user")]
    public User? User { get; set; }

    [JsonPropertyName("comments_url")]
    public string? CommentsUrl { get; set; }

    [JsonPropertyName("owner")]
    public User? Owner { get; set; }

    [JsonPropertyName("forks")]
    public object[] Forks { get; set; } = new object[0];

    [JsonPropertyName("history")]
    public Commit[] History { get; set; } = new Commit[0];

    [JsonPropertyName("truncated")]
    public bool Truncated { get; set; }
}
