using CodeJanitor.Platform.Application.Responses;
using MediatR;

namespace CodeJanitor.Platform.Application.Commands;

public sealed record RegisterRepositoryCommand : IRequest<RegisterRepositoryResponse>
{
    public required string RepositoryUrl { get; init; }
    public required string AccessToken { get; init; }
}