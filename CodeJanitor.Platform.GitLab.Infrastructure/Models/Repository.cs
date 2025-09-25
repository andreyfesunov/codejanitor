using CodeJanitor.Platform.Domain.ValueObjects;
using DomainRepository = CodeJanitor.Platform.Domain.Models.Repository;

namespace CodeJanitor.Platform.GitLab.Infrastructure.Models;

public sealed class Repository(
    Guid id,
    string url,
    string token
) : DomainRepository(
    id,
    RepositoryUrl.Create(url),
    RepositoryToken.Create(token)
);