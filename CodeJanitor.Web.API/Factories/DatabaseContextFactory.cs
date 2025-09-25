using CodeJanitor.Shared.Infrastructure.Factories;
using Microsoft.Extensions.Options;

namespace CodeJanitor.Web.API.Factories;

public sealed record DatabaseContextOptions
{
    public required string ConnectionString { get; init; }
}

public sealed class DatabaseContextFactory(IOptions<DatabaseContextOptions> options) : IDatabaseContextFactory
{
    public Task<string> GetConnectionString()
    {
        return Task.FromResult(options.Value.ConnectionString);
    }
}