using CodeJanitor.Platform.Application.Commands;
using CodeJanitor.Platform.Domain.Factories;
using CodeJanitor.Platform.Domain.Models;
using CodeJanitor.Shared.Domain.Events;
using MediatR;

namespace CodeJanitor.Platform.Application.Handlers.GitLab;

public sealed class ProcessIssueCommandHandler(
    IMediator mediator,
    IPlatformRepositoryFactory repositoryFactory
) : IRequestHandler<ProcessIssueCommand, Unit>
{
    public async Task<Unit> Handle(ProcessIssueCommand request, CancellationToken cancellationToken)
    {
        // TODO move to validity
        if (!request.Labels.Contains("codejanitor")) return Unit.Value;

        var repository = repositoryFactory.Create(Provider.GitLab);
        var model = await repository.RequireByIdAsync(request.RepositoryId);

        var task = new IssueReceivedEvent
        {
            Id = model.Id,
            RepositoryUrl = model.Url.Value.ToString(),
            AccessToken = model.Token.Value,
            Labels = request.Labels,
            Title = request.Title,
            Description = request.Description
        };
        await mediator.Publish(task, cancellationToken);

        return Unit.Value;
    }
}