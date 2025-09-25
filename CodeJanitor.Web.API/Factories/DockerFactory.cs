using CodeJanitor.Docker.Infrastructure.Factories;
using Microsoft.Extensions.Options;

namespace CodeJanitor.Web.API.Factories;

public sealed class DockerOptions
{
    public required string Image { get; init; }
}

public sealed class DockerFactory(IOptions<DockerOptions> options) : IDockerFactory
{
    public Task<string> CreateImageReference()
    {
        return Task.FromResult(options.Value.Image);
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