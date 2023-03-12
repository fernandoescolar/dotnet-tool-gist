namespace DotnetGist.Actions;

public abstract class ActionBase : IAction
{
    private readonly Project _project;
    private readonly IGistService _gistService;

    protected ActionBase(Project project, IGistService gistService)
    {
        _project = project;
        _gistService = gistService;
    }

    public abstract Task ExecuteAsync();

    protected IEnumerable<GistReferenceItem> GetProjectGists()
    {
        return _project.GetGistReferences();
    }

    protected void AddGistToProject(GistReferenceItem gistReference)
    {
        _project.AddGistReference(gistReference);
        _project.Save();
    }

    protected void RemoveGistFromProject(GistReferenceItem gistReference)
    {
        _project.DeleteGistReference(gistReference);
        _project.Save();
    }

    protected record GistRecord(Gist Gist, GistReferenceItem GistReference, IEnumerable<GistFile> Files);

    protected Task<Gist?> GetGistAsync(string gistId, string? version = null)
        => _gistService.GetGistAsync(gistId, version);

    protected async Task<GistRecord> GetGistFilesAsync(GistReferenceItem gistReference)
    {
        var gist = await GetGistAsync(gistReference.Id ?? string.Empty, gistReference.Version);
        if (gist is null)
        {
            throw new Exception($"Gist {gistReference.Id}:{gistReference.Version} not found");
        }

        var files = gist.Files.Select(f => f.Key).ToList();
        if (gistReference.FilePattern is not null)
        {
            var matcher = new Matcher();
            matcher.AddInclude(gistReference.FilePattern);
            files = matcher.Match(files).Files.Select(f => f.Path).ToList();
        }

        return new GistRecord(gist, gistReference, files.Select(f => gist.Files[f]));
    }

    protected async Task DownloadGistFilesAsync(GistReferenceItem gistReference)
    {
        var g = await GetGistFilesAsync(gistReference);
        if (!g.Files.Any())
        {
            throw new Exception($"No files found for gist {gistReference.Id}:{gistReference.Version}");
        }

        if (g.Gist.Description is not null)
        {
            Console.WriteLine($"{g.Gist.Description}");
        }

        foreach (var file in g.Files)
        {
            if (file.Content is null)
            {
                throw new Exception($"File {file.Filename} is empty");
            }

            var path = GetWorkspacePath(gistReference, file);
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            Console.WriteLine($"  Downloading {file.Filename}");
            System.IO.File.WriteAllText(path, file.Content);
        }
    }

    protected async Task RemoveGistFilesAsync(GistReferenceItem gistReference)
    {
        var g = await GetGistFilesAsync(gistReference);
        foreach (var file in g.Files)
        {
            var path = GetWorkspacePath(gistReference, file);
            if (System.IO.File.Exists(path))
            {
                Console.WriteLine($"  Removing {file.Filename}");
                System.IO.File.Delete(path);
            }
        }
    }

    protected string GetWorkspacePath(GistReferenceItem gistReference, GistFile file)
    {
        if (file.Filename is null)
        {
            throw new Exception($"File name is empty");
        }

        if (gistReference.Id is null)
        {
            throw new Exception($"Gist id is empty");
        }

        if (gistReference.Version is null)
        {
            throw new Exception($"Gist version is empty");
        }

        var workspace = _project.FilePath is not null ? Path.GetDirectoryName(_project.FilePath) : Environment.CurrentDirectory;
        if (gistReference.OutputPath is not null)
        {
            return Path.Combine(workspace ?? string.Empty, gistReference.OutputPath, file.Filename);
        }

        return Path.Combine(workspace ?? string.Empty, "gist", gistReference.Id, gistReference.Version, file.Filename);
    }
}