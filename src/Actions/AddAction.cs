namespace DotnetGist.Actions;

public class AddAction : ActionBase
{
    private readonly string _gistId;
    private string? _gistVersion;
    private readonly string? _filePattern;
    private readonly string? _outputPath;

    public AddAction(Project project, IGistService gistService, string gistId, string? gistVersion, string? filePattern, string? outputPath) : base(project, gistService)
    {
        _gistId = gistId;
        _gistVersion = gistVersion;
        _filePattern = filePattern;
        _outputPath = outputPath;
    }

    public override async Task ExecuteAsync()
    {
        var gist = await GetGistAsync(_gistId, _gistVersion);
        if (gist is null)
        {
            throw new Exception($"Gist {_gistId} not found");
        }

        if (_gistVersion is null)
        {
            _gistVersion = gist.History.First().Version;
        }

        var gistReferences = GetProjectGists();
        if (gistReferences.Any(g => g.Id == _gistId))
        {
            throw new Exception($"Gist {_gistId} already exists");
        }

        var gistReference = new GistReferenceItem
        {
            Id = _gistId,
            Version = _gistVersion,
            FilePattern = _filePattern,
            OutputPath = _outputPath
        };

        Console.WriteLine($"Adding {_gistId}:{_gistVersion}...");
        await DownloadGistFilesAsync(gistReference);
        AddGistToProject(gistReference);
    }
}