using CodeJanitor.Platform.Domain.Models;

namespace CodeJanitor.Platform.Application.Responses;

public sealed record RegisterRepositoryResponse
{
    public required Repository Repository { get; init; }
    public required Workflow Workflow { get; init; }
}