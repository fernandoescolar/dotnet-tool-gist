namespace DotnetGist.Core.Exceptions;

public class NoGistFilesFoundException : Exception
{
    public NoGistFilesFoundException(GistReferenceItem gistReference) : base($"No files found for gist {gistReference.Id}:{gistReference.Version}")
    {
    }
}
