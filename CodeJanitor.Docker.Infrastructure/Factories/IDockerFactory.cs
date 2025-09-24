namespace CodeJanitor.Docker.Infrastructure.Factories;

public interface IDockerFactory
{
    public Task<string> CreateImageReference();
    public Task<string> CreateContainerReference(Guid repositoryId);
    public Task<string> CreateRunnerReference();
}