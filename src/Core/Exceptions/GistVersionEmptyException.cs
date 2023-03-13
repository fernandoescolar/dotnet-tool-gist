namespace DotnetGist.Core.Exceptions;

public class GistVersionEmptyException : Exception
{
    public GistVersionEmptyException() : base("Gist version is empty")
    {
    }
}
