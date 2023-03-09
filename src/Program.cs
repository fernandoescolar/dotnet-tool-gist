using System.CommandLine;
using System.CommandLine.Invocation;

var root = new RootCommand(@"$ dotnet gist");
var projectArgument = new Argument<string?>("project", "The project file to use");
var gistIdArgument = new Argument<string>("gist_id", "The gist id");
var gistIdOptionalArgument = new Argument<string?>("gist_id", () => default, "The gist id");
var versionOption  = new Option<string>(new [] { "-v", "--version" }, "The gist version");
var fileOption  = new Option<string>(new [] { "-f", "--file" }, "The file filter glob pattern");
var outOption  = new Option<string>(new [] { "-o", "--out" }, "The output path");
var allOption  = new Option<bool>(new [] { "-a", "--all" }, "Update all references");

var addCommand = new Command("add", "Add gist reference to the project");
addCommand.AddArgument(gistIdArgument);
addCommand.AddOption(versionOption);
addCommand.AddOption(fileOption);
addCommand.AddOption(outOption);

var removeCommand = new Command("remove", "Remove gist reference from the project");
removeCommand.AddArgument(gistIdArgument);

var listCommand = new Command("list", "List gist references in the project");

var updateCommand = new Command("update", "Update gist reference in the project");
updateCommand.AddArgument(gistIdOptionalArgument);
updateCommand.AddOption(versionOption);
updateCommand.AddOption(allOption);

var restoreCommand = new Command("restore", "Restore gist references in the project");

root.AddArgument(projectArgument);
root.AddCommand(addCommand);
root.AddCommand(removeCommand);
root.AddCommand(listCommand);
root.AddCommand(updateCommand);
root.AddCommand(restoreCommand);

var actionFactory = new ActionFactory();

root.SetHandler(Handler);
addCommand.SetHandler(HandlerAsync);
removeCommand.SetHandler(HandlerAsync);
listCommand.SetHandler(HandlerAsync);
updateCommand.SetHandler(HandlerAsync);
restoreCommand.SetHandler(HandlerAsync);

await root.InvokeAsync(args);

string? FindProjectFile()
{
    var files = ProjectFinder.Run();
    if (files.Length == 0)
    {
        Error("No project file found");
        return default;
    }

    if (files.Length > 1)
    {
        Error("Multiple project files found");
        return default;
    }

    return files.First();
}

void Error(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(message);
    Console.ResetColor();
}

void Handler(InvocationContext ctx)
{
    var task = HandlerAsync(ctx);
    task.Wait();
}

async Task HandlerAsync (InvocationContext ctx)
{
    var project = ctx.ParseResult.GetValueForArgument<string?>(projectArgument);
    if (project is null)
    {
        project = FindProjectFile();
        if (project is null)
        {
            return;
        }
    }

    var command = ctx.ParseResult.CommandResult.Command.Name;
    var gistId = ctx.ParseResult.GetValueForArgument<string>(gistIdArgument);
    var version = ctx.ParseResult.GetValueForOption<string>(versionOption);
    var file = ctx.ParseResult.GetValueForOption<string>(fileOption);
    var output = ctx.ParseResult.GetValueForOption<string>(outOption);
    var all = ctx.ParseResult.GetValueForOption<bool>(allOption);
    var actionFactory = new ActionFactory();
    var action = command switch {
        "add"     => actionFactory.Add(project, gistId, version, file, output),
        "remove"  => actionFactory.Remove(project, gistId, version),
        "list"    => actionFactory.List(project),
        "update"  => all ? actionFactory.UpdateAll(project) : actionFactory.Update(project, gistId, version),
        "restore" => actionFactory.Restore(project),
        _ => default
    };

    if (action is null)
    {
        Error("Unknown command");
        return;
    }

    try
    {
        await action.ExecuteAsync();
    }
    catch (Exception ex)
    {
        Error(ex.Message);
    }
}
