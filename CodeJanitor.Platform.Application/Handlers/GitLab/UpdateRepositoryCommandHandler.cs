using CodeJanitor.Platform.Application.Commands;
using CodeJanitor.Platform.Application.Responses;
using CodeJanitor.Platform.Domain.Exceptions;
using CodeJanitor.Platform.Domain.Models;
using CodeJanitor.Platform.Domain.Repositories;
using CodeJanitor.Platform.Domain.ValueObjects;
using MediatR;

namespace CodeJanitor.Platform.Application.Handlers.GitLab;

public sealed class UpdateRepositoryCommandHandler(IPlatformRepository repository)
    : IRequestHandler<UpdateRepositoryCommand, UpdateRepositoryResponse>
{
    public async Task<UpdateRepositoryResponse> Handle(UpdateRepositoryCommand request,
        CancellationToken cancellationToken)
    {
        var url = RepositoryUrl.Create(request.RepositoryUrl);
        var previousToken = RepositoryToken.Create(request.PreviousAccessToken, Provider.GitLab);
        var newToken = RepositoryToken.Create(request.NewAccessToken, Provider.GitLab);

        var oldModel = await repository.GetByUrl(url);

        if (oldModel == null) throw new RepositoryNotExistsException(url);
        if (oldModel.Token != previousToken) throw new RepositoryUpdateFailedException(url);

        var newModel = new Repository(url, newToken);

        await repository.Validate(newModel);

        return new UpdateRepositoryResponse
        {
            Repository = await repository.Update(newModel)
        };
    }
}