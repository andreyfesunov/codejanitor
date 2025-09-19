using CodeJanitor.Platform.Domain.Models;
using CodeJanitor.Platform.Domain.Repositories;
using CodeJanitor.Platform.Domain.ValueObjects;
using CodeJanitor.Platform.GitLab.Infrastructure.Utilities;
using CodeJanitor.Shared.Infrastructure.Contexts;
using Dapper;

namespace CodeJanitor.Platform.GitLab.Infrastructure.Repositories;

public sealed class PlatformRepository(DatabaseContext context) : IPlatformRepository
{
    public async Task<Repository?> GetByUrl(RepositoryUrl url)
    {
        return await context.ExecuteAsync(async connection =>
        {
            var result = await connection.QueryAsync<Repository>(
                "SELECT * FROM repositories WHERE url = @Url",
                new { Url = url.Value }
            );

            return result.FirstOrDefault();
        });
    }

    public async Task<Repository> Create(Repository repository)
    {
        return await context.ExecuteAsync(async connection =>
        {
            await connection.ExecuteAsync(
                "INSERT INTO repositories (id, url, token) VALUES (@Id, @Url, @Token)",
                new { repository.Id, Url = repository.Url.Value, Token = repository.Token.Value }
            );

            return repository;
        });
    }

    public async Task<Repository> Update(Repository repository)
    {
        return await context.ExecuteAsync(async connection =>
        {
            await connection.ExecuteAsync(
                "UPDATE repositories SET url = @Url, token = @Token WHERE id = @Id",
                new { repository.Id, Url = repository.Url.Value, Token = repository.Token.Value }
            );

            return repository;
        });
    }

    public async Task Validate(Repository repository)
    {
        var validator = new TokenValidator(repository.Url.Value.ToString(), repository.Token.Value);

        await validator.ValidateTokenAndPermissionsAsync();
    }

    public Task<Workflow> GetWorkflow(Repository repository)
    {
        throw new NotImplementedException();
    }
}