using CodeJanitor.Platform.Application.Commands;
using CodeJanitor.Shared.Domain.Events;
using MediatR;

namespace CodeJanitor.Platform.Application.Handlers.GitLab;

public sealed class ProcessIssueCommandHandler(IMediator mediator) : IRequestHandler<ProcessIssueCommand, Unit>
{
    public async Task<Unit> Handle(ProcessIssueCommand request, CancellationToken cancellationToken)
    {
        // TODO move to validity
        if (!request.Labels.Contains("code-janitor")) return Unit.Value;

        var task = new IssueReceivedEvent
        {
            Labels = request.Labels,
            Title = request.Title,
            Description = request.Description
        };
        await mediator.Publish(task, cancellationToken);

        return Unit.Value;
    }
}