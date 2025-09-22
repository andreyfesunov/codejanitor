using CodeJanitor.Shared.Domain.Events;
using MediatR;

namespace CodeJanitor.Docker.Application.Handlers;

public sealed class IssueReceivedEventHandler : INotificationHandler<IssueReceivedEvent>
{
    public Task Handle(IssueReceivedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}