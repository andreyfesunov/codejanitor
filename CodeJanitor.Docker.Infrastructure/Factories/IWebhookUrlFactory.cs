namespace CodeJanitor.Docker.Infrastructure.Factories;

public interface IWebhookUrlFactory
{
    public Task<string> CreateDockerUrl();
}