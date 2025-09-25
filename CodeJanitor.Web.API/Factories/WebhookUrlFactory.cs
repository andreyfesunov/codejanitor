using Microsoft.Extensions.Options;
using GitLabFactories = CodeJanitor.Platform.GitLab.Infrastructure.Factories;
using DockerFactories = CodeJanitor.Docker.Infrastructure.Factories;

namespace CodeJanitor.Web.API.Factories;

public sealed class WebhookOptions
{
    public required string GitLabEndpoint { get; init; }
    public required string DockerEndpoint { get; init; }
}

public sealed class WebhookUrlFactory(
    IOptions<WebhookOptions> options,
    IHttpContextAccessor accessor
) : GitLabFactories.IWebhookUrlFactory, DockerFactories.IWebhookUrlFactory
{
    public Task<string> CreateGitLabUrl(Guid repositoryId)
    {
        var host = accessor.HttpContext?.Request.Host.ToString() ?? "localhost";
        var scheme = accessor.HttpContext?.Request.Scheme ?? "http";
        var url = $"{scheme}://{host}{options.Value.GitLabEndpoint}?repositoryId={repositoryId.ToString()}";

        return Task.FromResult(url);
    }

    public Task<string> CreateDockerUrl()
    {
        var host = accessor.HttpContext?.Request.Host.ToString() ?? "localhost";
        var scheme = accessor.HttpContext?.Request.Scheme ?? "http";
        var url = $"{scheme}://{host}{options.Value.DockerEndpoint}";

        return Task.FromResult(url);
    }
}