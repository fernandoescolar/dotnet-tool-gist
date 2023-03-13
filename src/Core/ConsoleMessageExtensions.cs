namespace DotnetGist.Core;

public static class ConsoleMessageExtensions
{
    public static void GistDescription(this IConsole console, Gist gist, string workingfolder)
    {
        if (gist.Description is not null)
        {
            console.WriteLine($"Description: {gist.Description}");
        }

        if (!string.IsNullOrWhiteSpace(workingfolder))
        {
            console.WriteLine($"Output path: {workingfolder}");
        }

        console.WriteLine($"Files:");
    }

    public static void GistFile(this IConsole console, GistFile file)
    {
        console.WriteLine($"  {file.Filename}");
    }

    public static void GistFileIsEmpty(this IConsole console, GistFile file)
    {
        console.WriteLine($"  Ignoring {file.Filename}: is empty");
    }

    public static void DownloadingGistFile(this IConsole console, GistFile file)
    {
        console.WriteLine($"  Downloading {file.Filename}");
    }

    public static void RemovingGistFile(this IConsole console, GistFile file)
    {
        console.WriteLine($"  Removing {file.Filename}");
    }

    public static void GistReference(this IConsole console, GistReferenceItem gistReference)
    {
        console.WriteLine(string.Empty);
        console.WriteLine($"Id: {gistReference.Id}");
        console.WriteLine($"Version: {gistReference.Version}");
    }

    public static void AddingGistReference(this IConsole console, GistReferenceItem gistReference)
    {
        console.WriteLine($"Adding {gistReference.Id}:{gistReference.Version}...");
    }

    public static void RemovingGistReference(this IConsole console, GistReferenceItem gistReference)
    {
        console.WriteLine($"Removing {gistReference.Id}:{gistReference.Version}...");
    }

    public static void RestoringGistReference(this IConsole console, GistReferenceItem gistReference)
    {
        console.WriteLine($"Restoring {gistReference.Id}:{gistReference.Version}...");
    }

    public static void UpdatingGistReference(this IConsole console, GistReferenceItem gistReference)
    {
        console.WriteLine($"Updating {gistReference.Id} to {gistReference.Version}...");
    }

    public static void NoGistsFound(this IConsole console)
    {
        console.WriteLine("No gist references found.");
    }

    public static void NoUpdatesFound(this IConsole console)
    {
        console.WriteLine("The gist reference is up to date.");
    }

    public static void AnyUpdateFound(this IConsole console)
    {
        console.WriteLine("All gist references are up to date.");
    }

    public static void UnknownCommand(this IConsole console)
    {
        console.WriteErrorLine("Unknown command");
    }

    public static void NoProject(this IConsole console)
    {
        console.WriteErrorLine("No project file found");
    }

    public static void MultipleProjects(this IConsole console)
    {
        console.WriteErrorLine("Multiple project files found");
    }

    public static void Exception(this IConsole console, Exception ex)
    {
        console.WriteErrorLine(ex.Message);
    }
}
