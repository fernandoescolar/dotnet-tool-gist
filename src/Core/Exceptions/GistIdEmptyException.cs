namespace DotnetGist.Core.Exceptions;

public class GistIdEmptyException : Exception
{
    public GistIdEmptyException() : base("Gist id is empty")
    {
    }
}
