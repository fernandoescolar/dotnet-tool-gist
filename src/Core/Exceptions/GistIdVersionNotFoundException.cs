namespace DotnetGist.Core.Exceptions;

public class GistIdVersionNotFoundException : Exception
{
    public GistIdVersionNotFoundException(GistReferenceItem gistReference) : this(gistReference.Id ?? string.Empty, gistReference.Version ?? string.Empty)
    {
    }

    public GistIdVersionNotFoundException(string gistId, string gistVersion) : base($"Gist {gistId} with version {gistVersion} not found")
    {
    }
}
