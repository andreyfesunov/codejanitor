using CodeJanitor.Platform.Domain.Models;

namespace CodeJanitor.Platform.Domain.Exceptions;

public class InvalidPlatformTokenException(Provider provider)
    : Exception($"Invalid format of {nameof(provider)} token.");