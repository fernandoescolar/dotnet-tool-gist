namespace DotnetGist.Actions;

public class RemoveAction : ActionBase
{
    private readonly string _gistId;
    private readonly string? _gistVersion;

    public RemoveAction(Project project, IGistService gistService, string gistId, string? gistVersion) : base(project, gistService)
    {
        _gistId = gistId;
        _gistVersion = gistVersion;
    }

    public override async Task ExecuteAsync()
    {
        var gistReferences = GetProjectGists();
        gistReferences = gistReferences.Where(g => g.Id == _gistId).ToList();
        if (!gistReferences.Any())
        {
            throw new Exception($"Gist {_gistId} not found");
        }

        if (_gistVersion is not null)
        {
            gistReferences = gistReferences.Where(g => g.Version == _gistVersion).ToList();
            if (!gistReferences.Any())
            {
                throw new Exception($"Gist {_gistId} version {_gistVersion} not found");
            }
        }

        var gists = new Dictionary<GistReferenceItem, Gist>();
        foreach (var gistReference in gistReferences)
        {
            Console.WriteLine($"Removing {_gistId}:{gistReference.Version}...");
            await RemoveGistFilesAsync(gistReference);
            RemoveGistFromProject(gistReference);
        }
    }
}