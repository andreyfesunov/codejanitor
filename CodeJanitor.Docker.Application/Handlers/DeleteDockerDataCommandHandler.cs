using CodeJanitor.Docker.Application.Commands;
using CodeJanitor.Docker.Domain.Repositories;
using MediatR;

namespace CodeJanitor.Docker.Application.Handlers;

public sealed class DeleteDockerDataCommandHandler(IDockerRepository repository)
    : IRequestHandler<DeleteDockerDataCommand, Unit>
{
    public async Task<Unit> Handle(DeleteDockerDataCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteContainerAsync(request.ContainerId);

        return Unit.Value;
    }
}