using MediatR;

namespace CodeJanitor.Shared.Domain.Events;

public sealed record IssueReceivedEvent : INotification
{
    public required Guid Id { get; init; }
    public required string RepositoryUrl { get; init; }
    public required string AccessToken { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required IEnumerable<string> Labels { get; init; }
}