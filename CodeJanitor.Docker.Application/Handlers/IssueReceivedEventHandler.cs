using CodeJanitor.Docker.Domain.Repositories;
using CodeJanitor.Shared.Domain.Events;
using MediatR;

namespace CodeJanitor.Docker.Application.Handlers;

public sealed class IssueReceivedEventHandler(IDockerRepository repository) : INotificationHandler<IssueReceivedEvent>
{
    public async Task Handle(IssueReceivedEvent notification, CancellationToken cancellationToken)
    {
        await repository.CreateBugFixContainerAsync(
            notification.Id,
            notification.RepositoryUrl,
            notification.AccessToken,
            notification.Title,
            notification.Description
        );
    }
}