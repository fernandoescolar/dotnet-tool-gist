namespace DotnetGist.Msbuild;

public class ProjectFinder
{
    public static string[] Run(string? directory = null)
    {
        directory ??= Directory.GetCurrentDirectory();

        var patterns = "*.csproj;*.vbproj;*.fsproj";
        foreach(var pattern in patterns.Split(';'))
        {
            var files = Directory.GetFiles(directory, pattern);
            if(files.Length > 0)
            {
                return files;
            }
        }

        return new string[0];
    }
}