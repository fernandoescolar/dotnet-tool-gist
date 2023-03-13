namespace DotnetGist.Core;

public class CommandContext
{
    public CommandContext(InvocationContext invocationContext, Project project, IGistService gistService)
    {
        InvocationContext = invocationContext;
        Project = project;
        GistService = gistService;
    }

    public InvocationContext InvocationContext { get; }

    public Project Project { get; }

    public IGistService GistService { get; }

    public ParseResult ParseResult => InvocationContext.ParseResult;

    public IConsole Console => InvocationContext.Console;
}
