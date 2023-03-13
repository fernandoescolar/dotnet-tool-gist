namespace DotnetGist.Core;

public static class ConsoleExtensions
{
    public static void WriteError(this IConsole console, string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        console.Error.Write(message);
        Console.ResetColor();
    }

    public static void WriteErrorLine(this IConsole console, string message)
        => WriteError(console, $"{message}\n");
}
