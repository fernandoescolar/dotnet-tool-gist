namespace DotnetGist.Actions;

public class RestoreAction : ActionBase
{
    public RestoreAction(Project project, IGistService gistService): base(project, gistService)
    {
    }

    public override async Task ExecuteAsync()
    {
        var gistReferences = GetProjectGists();
        if (!gistReferences.Any())
        {
            Console.WriteLine("No gists found");
        }

        foreach (var gistReference in gistReferences)
        {
            Console.WriteLine($"Restoring {gistReference.Id}:{gistReference.Version}...");
            await DownloadGistFilesAsync(gistReference);
        }
    }
}