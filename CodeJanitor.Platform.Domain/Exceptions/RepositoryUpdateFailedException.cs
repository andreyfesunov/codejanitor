using CodeJanitor.Platform.Domain.ValueObjects;

namespace CodeJanitor.Platform.Domain.Exceptions;

public sealed class RepositoryUpdateFailedException(RepositoryUrl url)
    : Exception($"Failed to update repository with URL: {url.Value.ToString()}. Incorrect data provided.");