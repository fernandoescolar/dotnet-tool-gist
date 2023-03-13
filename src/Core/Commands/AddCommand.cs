namespace DotnetGist.Core.Commands;

public class AddCommand : CommandBase
{
    private readonly Argument<string> _gistIdArgument;
    private readonly Option<string> _versionOption;
    private readonly Option<string> _fileOption;
    private readonly Option<string> _outOption;

    public AddCommand() : base("add", "Add gist reference to the project")
    {
        _gistIdArgument = new Argument<string>("gist_id", "The gist id");
        _versionOption  = new Option<string>(new [] { "-v", "--version" }, "The gist version");
        _fileOption  = new Option<string>(new [] { "-f", "--file" }, "The file filter glob pattern");
        _outOption  = new Option<string>(new [] { "-o", "--out" }, "The output path");

        AddArgument(_gistIdArgument);
        AddOption(_versionOption);
        AddOption(_fileOption);
        AddOption(_outOption);
    }

    protected override async Task OnHandleAsync(CommandContext ctx)
    {
        var _gistId = ctx.ParseResult.GetValueForArgument<string>(_gistIdArgument);
        var _gistVersion = ctx.ParseResult.GetValueForOption<string>(_versionOption);
        var _filePattern = ctx.ParseResult.GetValueForOption<string>(_fileOption);
        var _outputPath = ctx.ParseResult.GetValueForOption<string>(_outOption);


        var gist = await ctx.GetGistAsync(_gistId, _gistVersion);
        if (gist is null)
        {
            throw new GistIdNotFoundException(_gistId);
        }

        if (_gistVersion is null)
        {
            _gistVersion = gist.History.First().Version;
        }

        var gistReferences = ctx.GetProjectGists();
        if (gistReferences.Any(g => g.Id == _gistId))
        {
            throw new GistIdAlreadyExistsException(_gistId);
        }

        var gistReference = new GistReferenceItem
        {
            Id = _gistId,
            Version = _gistVersion,
            FilePattern = _filePattern,
            OutputPath = _outputPath
        };

        ctx.Console.AddingGistReference(gistReference);
        await ctx.DownloadGistFilesAsync(gistReference);
        ctx.AddGistToProject(gistReference);
    }
}
