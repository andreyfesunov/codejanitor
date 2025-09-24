namespace CodeJanitor.Platform.GitLab.Infrastructure.Factories;

public interface IWebhookUrlFactory
{
    public Task<string> CreateGitLabUrl(Guid repositoryId);
}