namespace DotnetGist.Infrastructure.Github;

public interface IGistCache
{
    Task<Gist?> GetGistAsync(string gistId, string? version = null);
    Task SetGistAsync(Gist gist);
}