using CodeJanitor.Platform.Domain.Factories;
using CodeJanitor.Platform.Domain.Models;
using CodeJanitor.Platform.Domain.Repositories;
using GitLab = CodeJanitor.Platform.GitLab.Infrastructure.Repositories;

namespace CodeJanitor.Web.API.Factories;

public sealed class PlatformRepositoryFactory(IServiceProvider serviceProvider) : IPlatformRepositoryFactory
{
    public IPlatformRepository Create(Provider provider)
    {
        return provider switch
        {
            Provider.GitLab => serviceProvider.GetRequiredService<GitLab.PlatformRepository>(),
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };
    }
}