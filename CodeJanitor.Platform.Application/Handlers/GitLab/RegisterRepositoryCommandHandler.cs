using CodeJanitor.Platform.Application.Commands;
using CodeJanitor.Platform.Application.Responses;
using CodeJanitor.Platform.Domain.Exceptions;
using CodeJanitor.Platform.Domain.Factories;
using CodeJanitor.Platform.Domain.Models;
using CodeJanitor.Platform.Domain.ValueObjects;
using MediatR;

namespace CodeJanitor.Platform.Application.Handlers.GitLab;

public sealed class RegisterRepositoryCommandHandler(IPlatformRepositoryFactory factory)
    : IRequestHandler<RegisterRepositoryCommand, RegisterRepositoryResponse>
{
    public async Task<RegisterRepositoryResponse> Handle(RegisterRepositoryCommand request,
        CancellationToken cancellationToken)
    {
        var url = RepositoryUrl.Create(request.RepositoryUrl);
        var token = RepositoryToken.Create(request.AccessToken, Provider.GitLab);

        var repository = factory.Create(Provider.GitLab);

        if (await repository.GetByUrlAsync(url) != null) throw new RepositoryAlreadyExistsException(url);

        cancellationToken.ThrowIfCancellationRequested();

        var model = new Repository(url, token);

        await repository.ValidateAsync(model);

        var createTask = repository.CreateAsync(model);
        var workflowTask = repository.GetWorkflowAsync(model);

        await Task.WhenAll(createTask, workflowTask);

        var workflow = workflowTask.Result;

        return new RegisterRepositoryResponse
        {
            Repository = model,
            Workflow = workflow
        };
    }
}