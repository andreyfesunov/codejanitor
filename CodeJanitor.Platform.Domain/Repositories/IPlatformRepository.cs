using CodeJanitor.Platform.Domain.Models;
using CodeJanitor.Platform.Domain.ValueObjects;

namespace CodeJanitor.Platform.Domain.Repositories;

public interface IPlatformRepository
{
    public Task<Repository?> GetByUrl(RepositoryUrl url);
    public Task<Repository> Create(Repository repository);
    public Task<Repository> Update(Repository repository);
    public Task<Workflow> GetWorkflow(Repository repository);
}