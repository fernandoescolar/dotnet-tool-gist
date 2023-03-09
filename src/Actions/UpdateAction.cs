namespace DotnetGist.Actions;

public class UpdateAction : ActionBase
{
    private readonly string? _gistId;
    private readonly string? _gistVersion;

    public UpdateAction(Project project, IGistService gistService, string gistId, string? gistVersion) : base(project, gistService)
    {
        _gistId = gistId;
        _gistVersion = gistVersion;
    }

    public UpdateAction(Project project, IGistService gistService) : base(project, gistService)
    {
    }

    public override async Task ExecuteAsync()
    {
        var gistReferences = GetProjectGists();
        if (!gistReferences.Any())
        {
            Console.WriteLine("No gists found");
        }

        if (_gistId is not null)
        {
            gistReferences = gistReferences.Where(g => g.Id == _gistId).ToList();
            if (!gistReferences.Any())
            {
                throw new Exception($"Gist {_gistId} not found");
            }
        }

        if (_gistVersion is not null)
        {
            gistReferences = gistReferences.Where(g => g.Version == _gistVersion).ToList();
            if (!gistReferences.Any())
            {
                throw new Exception($"Gist {_gistId} version {_gistVersion} not found");
            }
        }

        var hasUpdates = false;
        foreach (var gistReference in gistReferences)
        {
            var gist = await GetGistAsync(gistReference.Id ?? string.Empty);
            if (gist is null)
            {
                continue;
            }

            var newVersion = gist.History.First().Version;
            if (gistReference.Version == newVersion)
            {
                continue;
            }

            hasUpdates = true;
            Console.WriteLine($"Updating {_gistId} to {newVersion}...");
            await RemoveGistFilesAsync(gistReference);
            RemoveGistFromProject(gistReference);

            gistReference.Version = newVersion;
            await DownloadGistFilesAsync(gistReference);
            AddGistToProject(gistReference);
        }

        if (!hasUpdates)
        {
            Console.WriteLine("No updates found");
        }
    }
}