namespace CodeJanitor.Platform.GitLab.Infrastructure.Factories;

public interface IWebhookUrlFactory
{
    public Task<string> Create(Guid repositoryId);
}