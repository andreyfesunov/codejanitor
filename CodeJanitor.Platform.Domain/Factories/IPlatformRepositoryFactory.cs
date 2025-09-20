using CodeJanitor.Platform.Domain.Models;
using CodeJanitor.Platform.Domain.Repositories;

namespace CodeJanitor.Platform.Domain.Factories;

public interface IPlatformRepositoryFactory
{
    public IPlatformRepository Create(Provider provider);
}