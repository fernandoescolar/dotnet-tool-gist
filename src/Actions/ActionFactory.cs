namespace DotnetGist.Actions;

public class ActionFactory
{
    public IAction Add(string project, string gistId, string? gistVersion, string? filePattern, string? outputPath)
    {
        var p = new Project(project);
        return new AddAction(p, CreateGistService(p), gistId, gistVersion, filePattern, outputPath);
    }

    public IAction Remove(string project, string gistId, string? gistVersion)
    {
        var p = new Project(project);
        return new RemoveAction(p, CreateGistService(p), gistId, gistVersion);
    }

    public IAction List(string project)
    {
        var p = new Project(project);
        return new ListAction(p, CreateGistService(p));
    }

    public IAction Update(string project, string gistId, string? gistVersion)
    {
        var p = new Project(project);
        return new UpdateAction(p, CreateGistService(p), gistId, gistVersion);
    }

    public IAction UpdateAll(string project)
    {
        var p = new Project(project);
        return new UpdateAction(p, CreateGistService(p));
    }

    public IAction Restore(string project)
    {
        var p = new Project(project);
        return new RestoreAction(p, CreateGistService(p));
    }

    private IGistService CreateGistService(Project project)
    {
        var workingDirectory = Path.GetDirectoryName(project.FilePath);
        if (workingDirectory is null)
        {
            throw new Exception("Unable to determine working directory");
        }

        var objGistFolder = Path.Combine(workingDirectory, "obj", "gist");
        var gistCache = new GistFileSystemCache(objGistFolder);
        var httpClient = new HttpClient();

        return new GistService(httpClient, gistCache);
    }
}