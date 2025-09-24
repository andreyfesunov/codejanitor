using CodeJanitor.Docker.Infrastructure.Factories;

namespace CodeJanitor.Web.API.Factories;

public sealed class DockerOptions
{
    public required string Image { get; init; }
}

public sealed class DockerFactory(DockerOptions options) : IDockerFactory
{
    public Task<string> CreateImageReference()
    {
        return Task.FromResult(options.Image);
    }

    public Task<string> CreateContainerReference(Guid repositoryId)
    {
        return Task.FromResult($"codejanitor-{repositoryId.ToString()}");
    }

    public Task<string> CreateRunnerReference()
    {
        return Task.FromResult("../../CodeJanitor.Docker.Infrastructure.Runner");
    }
}