using CodeJanitor.Platform.Domain.Models;
using CodeJanitor.Platform.Domain.ValueObjects;

namespace CodeJanitor.Platform.Domain.Repositories;

public interface IPlatformRepository
{
    public Task<Repository?> GetByUrlAsync(RepositoryUrl url);
    public Task<Repository> RequireByIdAsync(Guid id);
    public Task<Repository> CreateAsync(Repository repository);
    public Task<Repository> UpdateAsync(Repository repository);
    public Task ValidateAsync(Repository repository);
    public Task<Workflow> GetWorkflowAsync(Repository repository);
}