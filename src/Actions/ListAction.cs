namespace DotnetGist.Actions;

public class ListAction : ActionBase
{
    public ListAction(Project project, IGistService gistService) : base(project, gistService)
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
            Console.WriteLine($"{gistReference.Id}:{gistReference.Version}");

            var gist = await GetGistAsync(gistReference.Id ?? string.Empty, gistReference.Version);
            if (gist?.Description is not null)
            {
                Console.WriteLine($"{gist.Description}");
            }

            var g = await GetGistFilesAsync(gistReference);
            foreach (var file in g.Files)
            {
                Console.WriteLine($"  {file.Filename}");
            }
        }
    }
}