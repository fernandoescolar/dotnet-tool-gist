namespace DotnetGist.Core;

public class MainCommand : RootCommand
{
    private readonly Argument<string?> _projectArgument;

    public MainCommand(IEnumerable<CommandBase> commands) : base("$ dotnet gist")
    {
        _projectArgument = new Argument<string?>("project", "The project file to use");
        foreach (var command in commands)
        {
            AddCommand(command);
            command.SetHandler(Handle);
        }

        this.SetHandler(Handle);
    }

    private void Handle(InvocationContext ctx)
    {
        var task = HandleAsync(ctx);
        task.Wait();
    }

    private async Task HandleAsync(InvocationContext ctx)
    {
        var project = ctx.ParseResult.GetValueForArgument<string?>(_projectArgument);
        if (project is null)
        {
            project = FindProjectFile(ctx.Console);
            if (project is null)
            {
                return;
            }
        }

        var command = ctx.ParseResult.CommandResult.Command;
        if (command is CommandBase commandBase)
        {
            var p = new Project(project);
            var workingDirectory = Path.Combine(Path.GetDirectoryName(p.FilePath) ?? string.Empty, "obj", ".gist");
            var cache = new GistFileSystemCache(workingDirectory);
            var service = new GistService(new HttpClient(), cache);
            var context = new CommandContext(ctx, p, service);
            try
            {
                await commandBase.HandleAsync(context);
            }
            catch (Exception ex)
            {
                ctx.Console.Exception(ex);
            }
        }
        else
        {
            ctx.Console.UnknownCommand();
        }
    }

    private static string? FindProjectFile(IConsole console)
    {
        var files = ProjectFinder.Run();
        if (files.Length == 0)
        {
            console.NoProject();
            return default;
        }

        if (files.Length > 1)
        {
            console.MultipleProjects();
            return default;
        }

        return files.First();
    }
}
