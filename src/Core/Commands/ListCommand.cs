namespace DotnetGist.Core.Commands;

public class ListCommand : CommandBase
{
    public ListCommand() : base("list", "List all gist references in the project")
    {
    }

    protected override async Task OnHandleAsync(CommandContext ctx)
    {
        var gistReferences = ctx.GetProjectGists();
        if (!gistReferences.Any())
        {
            ctx.Console.NoGistsFound();
        }

        foreach (var gistReference in gistReferences)
        {
            ctx.Console.GistReference(gistReference);

            var gist = await ctx.GetGistAsync(gistReference.Id ?? string.Empty, gistReference.Version);
            if (gist is not null)
            {
                ctx.Console.GistDescription(gist, ctx.GetWorkspacePath(gistReference));
            }

            var g = await ctx.GetGistFilesAsync(gistReference);
            foreach (var file in g.Files)
            {
                ctx.Console.GistFile(file);
            }
        }
    }
}