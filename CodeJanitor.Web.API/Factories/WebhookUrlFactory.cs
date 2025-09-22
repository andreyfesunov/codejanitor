using CodeJanitor.Platform.GitLab.Infrastructure.Factories;

namespace CodeJanitor.Web.API.Factories;

public sealed class WebhookOptions
{
    public required string Endpoint { get; init; }
}

public sealed class WebhookUrlFactory(
    WebhookOptions options,
    IHttpContextAccessor accessor
) : IWebhookUrlFactory
{
    public Task<string> Create(Guid repositoryId)
    {
        var host = accessor.HttpContext?.Request.Host.ToString() ?? "localhost";
        var scheme = accessor.HttpContext?.Request.Scheme ?? "http";
        var url = $"{scheme}://{host}{options.Endpoint}?repositoryId={repositoryId.ToString()}";

        return Task.FromResult(url);
    }
}