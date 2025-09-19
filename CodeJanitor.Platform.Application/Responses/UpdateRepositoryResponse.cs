using CodeJanitor.Platform.Domain.Models;

namespace CodeJanitor.Platform.Application.Responses;

public sealed record UpdateRepositoryResponse
{
    public required Repository Repository { get; init; }
}