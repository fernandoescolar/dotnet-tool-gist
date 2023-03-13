namespace DotnetGist.Core.Exceptions;

public class GistIdAlreadyExistsException : Exception
{
    public GistIdAlreadyExistsException(GistReferenceItem gistReference) : this(gistReference.Id ?? string.Empty)
    {
    }

    public GistIdAlreadyExistsException(string gistId) : base($"Gist {gistId} already exists")
    {
    }
}