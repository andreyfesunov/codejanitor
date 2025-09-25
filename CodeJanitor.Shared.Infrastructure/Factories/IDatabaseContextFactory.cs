namespace CodeJanitor.Shared.Infrastructure.Factories;

public interface IDatabaseContextFactory
{
    public Task<string> GetConnectionString();
}