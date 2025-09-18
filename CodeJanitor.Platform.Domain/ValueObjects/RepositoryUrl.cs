using CodeJanitor.Platform.Domain.Exceptions;

namespace CodeJanitor.Platform.Domain.ValueObjects;

public sealed record RepositoryUrl
{
    public readonly Uri Value;

    private RepositoryUrl(Uri value)
    {
        Value = value;
    }
    
    public static RepositoryUrl Create(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            throw new InvalidRepositoryUrlException(url);
        
        return new RepositoryUrl(uri);
    }
}