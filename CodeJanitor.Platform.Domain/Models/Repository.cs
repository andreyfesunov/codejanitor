using CodeJanitor.Platform.Domain.ValueObjects;

namespace CodeJanitor.Platform.Domain.Models;

public sealed class Repository(
    RepositoryUrl url,
    RepositoryToken token
)
{
    public readonly Guid Id = Guid.NewGuid();
    public readonly RepositoryUrl Url = url;
    public readonly RepositoryToken Token = token;
    public Provider Provider => Token.GetProvider();
}