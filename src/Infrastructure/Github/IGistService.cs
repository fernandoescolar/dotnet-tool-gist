namespace DotnetGist.Infrastructure.Github;

public interface IGistService
{
    Task<Gist?> GetGistAsync(string gistId, string? version = null);
}