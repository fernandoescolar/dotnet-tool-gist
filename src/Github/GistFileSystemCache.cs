namespace DotnetGist.Github;

public class GistFileSystemCache : IGistCache
{
    private readonly Dictionary<string, Gist> _cache = new();
    private readonly string _cacheDirectory;

    public GistFileSystemCache(string cacheDirectory)
    {
        _cacheDirectory = cacheDirectory;
    }

    public async Task<Gist?> GetGistAsync(string gistId, string? version = null)
    {
        if (string.IsNullOrEmpty(gistId) || string.IsNullOrEmpty(version))
        {
            return null;
        }

        var cacheFile = GetCacheFile(gistId, version);
        if (_cache.TryGetValue(cacheFile, out var gist))
        {
            return gist;
        }

        if (!File.Exists(cacheFile))
        {
            return null;
        }

        using var stream = File.OpenRead(cacheFile);
        return await JsonSerializer.DeserializeAsync<Gist>(stream);
    }

    public async Task SetGistAsync(Gist gist)
    {
        var id = gist.Id;
        var version = gist.History.First().Version;
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(version))
        {
            throw new ArgumentException("Gist must have an id and version");
        }

        var cacheFile = GetCacheFile(id, version);
        if (!Directory.Exists(_cacheDirectory))
        {
            Directory.CreateDirectory(_cacheDirectory);
        }

        _cache.Add(cacheFile, gist);

        using var stream = File.OpenWrite(cacheFile);
        await JsonSerializer.SerializeAsync(stream, gist);
    }

    private string GetCacheFile(string gistId, string version)
    {
        var fileName = $"{gistId}-{version}.gist";
        return Path.Combine(_cacheDirectory, fileName);
    }
}
