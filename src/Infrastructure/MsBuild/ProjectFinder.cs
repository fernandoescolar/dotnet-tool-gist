namespace DotnetGist.Infrastructure.MsBuild;

public static class ProjectFinder
{
    public static string[] Run(string? directory = null)
    {
        directory ??= Directory.GetCurrentDirectory();

        const string patterns = "*.csproj;*.vbproj;*.fsproj";
        foreach (var pattern in patterns.Split(';'))
        {
            var files = Directory.GetFiles(directory, pattern);
            if (files.Length > 0)
            {
                return files;
            }
        }

        return [];
    }
}