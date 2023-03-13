namespace DotnetGist.Infrastructure.Github;

public class GistService : IGistService
{
    private readonly HttpClient _httpClient;
    private readonly IGistCache _cache;

    public GistService(HttpClient httpClient, IGistCache cache)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "dotnet-gist");

        _cache = cache;
    }

    public async Task<Gist?> GetGistAsync(string gistId, string? version = null)
    {
        if (!string.IsNullOrEmpty(gistId))
        {
            var g = await _cache.GetGistAsync(gistId, version);
            if (g is not null)
            {
                return g;
            }
        }

        version = version is null ? string.Empty : $"/{version}";
        var gist = await GetGistByUrlAsync($"https://api.github.com/gists/{gistId}{version}");
        if (gist is not null)
        {
            await _cache.SetGistAsync(gist);
        }

        return gist;
    }

    private async Task<Gist?> GetGistByUrlAsync(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var gist = await JsonSerializer.DeserializeAsync<Gist>(stream);

        return gist;
    }
}
