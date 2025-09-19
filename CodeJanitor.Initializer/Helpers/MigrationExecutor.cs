using FluentMigrator.Runner;

namespace CodeJanitor.Initializer.Helpers;

public sealed class MigrationExecutor(IMigrationRunner runner)
{
    public Task RunAsync()
    {
        runner.MigrateUp();

        return Task.CompletedTask;
    }
}