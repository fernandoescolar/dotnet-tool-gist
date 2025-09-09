namespace DotnetGist.Infrastructure.MsBuild;

public class GistReferenceItem
{
    public string? Id { get; set; }

    public string? Version { get; set; }

    public string? FilePattern { get; set; }

    public string? OutputPath { get; set; }

    public string? Namespace { get; set; }
}