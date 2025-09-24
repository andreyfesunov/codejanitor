using MediatR;

namespace CodeJanitor.Platform.Application.Commands;

public sealed record ProcessIssueCommand : IRequest<Unit>
{
    public required Guid RepositoryId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required IEnumerable<string> Labels { get; init; }
}