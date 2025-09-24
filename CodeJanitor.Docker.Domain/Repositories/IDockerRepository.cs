namespace CodeJanitor.Docker.Domain.Repositories;

public interface IDockerRepository
{
    public Task CreateBugFixContainerAsync(
        Guid id,
        string url,
        string token,
        string title,
        string description
    );

    public Task DeleteContainerAsync(string containerId);
}