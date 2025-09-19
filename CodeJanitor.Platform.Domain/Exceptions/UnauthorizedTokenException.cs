namespace CodeJanitor.Platform.Domain.Exceptions;

public sealed class UnauthorizedTokenException(string message, Exception? e = null) : Exception(message, e);