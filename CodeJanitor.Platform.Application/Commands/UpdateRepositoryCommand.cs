using CodeJanitor.Platform.Application.Responses;
using MediatR;

namespace CodeJanitor.Platform.Application.Commands;

public sealed record UpdateRepositoryCommand : IRequest<UpdateRepositoryResponse>
{
    public required string RepositoryUrl { get; init; }
    public required string PreviousAccessToken { get; init; }
    public required string NewAccessToken { get; init; }
}