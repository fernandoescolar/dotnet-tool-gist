namespace DotnetGist.Core;

public static class CommandContextExtensions
{
    public static IEnumerable<GistReferenceItem> GetProjectGists(this CommandContext ctx)
    {
        return ctx.Project.GetGistReferences();
    }

    public static void AddGistToProject(this CommandContext ctx, GistReferenceItem gistReference)
    {
        ctx.Project.AddGistReference(gistReference);
        ctx.Project.Save();
    }

    public static void RemoveGistFromProject(this CommandContext ctx, GistReferenceItem gistReference)
    {
        ctx.Project.DeleteGistReference(gistReference);
        ctx.Project.Save();
    }


    public static Task<Gist?> GetGistAsync(this CommandContext ctx, string gistId, string? version = null)
        => ctx.GistService.GetGistAsync(gistId, version);

    public static async Task DownloadGistFilesAsync(this CommandContext ctx,GistReferenceItem gistReference)
    {
        var g = await ctx.GetGistFilesAsync(gistReference);
        if (!g.Files.Any())
        {
            throw new NoGistFilesFoundException(gistReference);
        }


        ctx.Console.GistDescription(g.Gist, ctx.GetWorkspacePath(gistReference));
        foreach (var file in g.Files)
        {
            if (file.Content is null)
            {
                ctx.Console.GistFileIsEmpty(file);
                continue;
            }

            var path = ctx.GetWorkspacePath(gistReference, file);
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            ctx.Console.DownloadingGistFile(file);
            System.IO.File.WriteAllText(path, file.Content);
        }
    }

    public static async Task RemoveGistFilesAsync(this CommandContext ctx, GistReferenceItem gistReference)
    {
        var g = await ctx.GetGistFilesAsync(gistReference);
        foreach (var file in g.Files)
        {
            var path = ctx.GetWorkspacePath(gistReference, file);
            if (System.IO.File.Exists(path))
            {
                ctx.Console.RemovingGistFile(file);
                System.IO.File.Delete(path);
            }
        }
    }

    public static string GetWorkspacePath(this CommandContext ctx, GistReferenceItem gistReference, GistFile? file = null)
    {
        if (gistReference.Id is null)
        {
            throw new GistIdEmptyException();
        }

        if (gistReference.Version is null)
        {
            throw new GistVersionEmptyException();
        }

        var workspace = ctx.Project.FilePath is not null ? Path.GetDirectoryName(ctx.Project.FilePath) : Environment.CurrentDirectory;
        if (gistReference.OutputPath is not null)
        {
            return Path.Combine(workspace ?? string.Empty, gistReference.OutputPath, file?.Filename ?? string.Empty);
        }

        return Path.Combine(workspace ?? string.Empty, "gist", gistReference.Id, gistReference.Version, file?.Filename ?? string.Empty);
    }

    internal record GistRecord(Gist Gist, GistReferenceItem GistReference, IEnumerable<GistFile> Files);

    internal static async Task<GistRecord> GetGistFilesAsync(this CommandContext ctx, GistReferenceItem gistReference)
    {
        var gist = await ctx.GetGistAsync(gistReference.Id ?? string.Empty, gistReference.Version);
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
}