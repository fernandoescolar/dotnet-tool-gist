namespace DotnetGist.Core;

public abstract class CommandBase : Command
{
    protected CommandBase(string name, string description = "") : base(name, description)
    {
    }

    public Task HandleAsync(CommandContext ctx)
    {
        return OnHandleAsync(ctx);
    }

    protected abstract Task OnHandleAsync(CommandContext ctx);
}
