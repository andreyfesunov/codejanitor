using CodeJanitor.Platform.Domain.ValueObjects;

namespace CodeJanitor.Platform.Domain.Exceptions;

public class RepositoryNotExistsException(RepositoryUrl url)
    : Exception($"Repository with URL '{url.Value.ToString()}' not exists.");