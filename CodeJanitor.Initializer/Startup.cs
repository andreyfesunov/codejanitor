using CodeJanitor.Initializer.Helpers;
using CodeJanitor.Initializer.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeJanitor.Initializer;

public static class Startup
{
    public static void Configure(HostBuilderContext context, IServiceCollection services)
    {
        RegisterServices(context, services);
    }

    private static void RegisterServices(HostBuilderContext context, IServiceCollection services)
    {
        var connectionString = context.Configuration.GetConnectionString("Default");

        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(CreateRepositoryTableMigration).Assembly).For.Migrations());

        services.AddScoped<CommandExecutor, CommandExecutor>();
        services.AddScoped<MigrationExecutor, MigrationExecutor>();
    }
}