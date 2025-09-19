namespace CodeJanitor.Platform.Domain.Exceptions;

public sealed class InvalidUrlException(string url) : Exception($"Invalid URL: '{url}'");