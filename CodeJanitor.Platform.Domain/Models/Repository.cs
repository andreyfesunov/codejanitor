using CodeJanitor.Platform.Domain.ValueObjects;

namespace CodeJanitor.Platform.Domain.Models;

public class Repository
{
    public readonly Guid Id = Guid.NewGuid();
    public readonly RepositoryToken Token;
    public readonly RepositoryUrl Url;

    public Repository(
        RepositoryUrl url,
        RepositoryToken token
    )
    {
        Url = url;
        Token = token;
    }

    protected Repository(
        Guid id,
        RepositoryUrl url,
        RepositoryToken token
    )
    {
        Id = id;
        Url = url;
        Token = token;
    }

    public Provider Provider => Token.GetProvider();
}