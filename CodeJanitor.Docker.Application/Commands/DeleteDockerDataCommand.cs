using MediatR;

namespace CodeJanitor.Docker.Application.Commands;

public sealed record DeleteDockerDataCommand : IRequest<Unit>
{
    public required string ContainerId { get; init; }
}