namespace DotnetGist.Core.Exceptions;

public class GistIdNotFoundException : Exception
{
    public GistIdNotFoundException(GistReferenceItem gistReference) : this(gistReference.Id ?? string.Empty)
    {
    }

    public GistIdNotFoundException(string gistId) : base($"Gist {gistId} not found")
    {
    }
}
