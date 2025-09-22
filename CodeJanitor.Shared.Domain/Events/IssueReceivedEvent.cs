using MediatR;

namespace CodeJanitor.Shared.Domain.Events;

public sealed record IssueReceivedEvent : INotification
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required IEnumerable<string> Labels { get; init; }
}