namespace DotnetGist.Core.Commands;

public class UpdateCommand : CommandBase
{
    private readonly Argument<string?> _gistIdArgument;
    private readonly Option<string> _versionOption;

    public UpdateCommand() : base("update", "Update gist reference in the project")
    {
        _gistIdArgument = new Argument<string?>("gist_id", () => default, "The gist id");
        _versionOption  = new Option<string>(new [] { "-v", "--version" }, "The gist version");

        AddArgument(_gistIdArgument);
        AddOption(_versionOption);
    }

    protected override async Task OnHandleAsync(CommandContext ctx)
    {
        var _gistId = ctx.ParseResult.GetValueForArgument<string?>(_gistIdArgument);
        var _gistVersion = ctx.ParseResult.GetValueForOption<string>(_versionOption);

        var gistReferences = ctx.GetProjectGists();
        if (_gistId is not null)
        {
            gistReferences = gistReferences.Where(g => g.Id == _gistId).ToList();
            if (!gistReferences.Any())
            {
                throw new GistIdNotFoundException(_gistId);
            }
        }

        if (_gistVersion is not null)
        {
            gistReferences = gistReferences.Where(g => g.Version == _gistVersion).ToList();
            if (!gistReferences.Any())
            {
                throw new GistIdVersionNotFoundException(_gistId ?? string.Empty, _gistVersion);
            }
        }

        var hasUpdates = false;
        foreach (var gistReference in gistReferences)
        {
            var gist = await ctx.GetGistAsync(gistReference.Id ?? string.Empty);
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
            ctx.Console.UpdatingGistReference(gistReference);
            await ctx.RemoveGistFilesAsync(gistReference);
            ctx.RemoveGistFromProject(gistReference);

            gistReference.Version = newVersion;
            await ctx.DownloadGistFilesAsync(gistReference);
            ctx.AddGistToProject(gistReference);
        }

        if (!hasUpdates)
        {
            if (_gistId is not null)
            {
                ctx.Console.NoUpdatesFound();
            }
            else
            {
                ctx.Console.AnyUpdateFound();
            }
        }
    }
}