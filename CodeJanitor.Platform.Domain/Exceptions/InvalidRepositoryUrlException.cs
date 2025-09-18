namespace CodeJanitor.Platform.Domain.Exceptions;

public sealed class InvalidRepositoryUrlException(string url) : Exception($"Invalid URL: '{url}'");