using CodeJanitor.Platform.Domain.ValueObjects;

namespace CodeJanitor.Platform.Domain.Exceptions;

public class RepositoryAlreadyExistsException(RepositoryUrl url) : Exception($"Repository with URL '{url.Value.ToString()}' already exists.");