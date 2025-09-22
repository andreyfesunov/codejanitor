using CodeJanitor.Docker.Application;
using CodeJanitor.Platform.Application;
using CodeJanitor.Platform.Domain.Factories;
using CodeJanitor.Platform.GitLab.Infrastructure.Factories;
using CodeJanitor.Shared.Infrastructure.Contexts;
using CodeJanitor.Web.API.Factories;
using GitLab = CodeJanitor.Platform.GitLab.Infrastructure.Repositories;

namespace CodeJanitor.Web.API;

public static class Startup
{
    public static void Configure(this IServiceCollection services, IConfigurationManager configuration)
    {
        #region Controllers (API)

        services.AddControllers();

        #endregion

        #region MediatR

        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(typeof(PlatformApplicationMarker).Assembly,
                typeof(DockerApplicationMarker).Assembly));

        #endregion

        #region Factories

        services.AddScoped<IPlatformRepositoryFactory, PlatformRepositoryFactory>();

        services.Configure<WebhookOptions>(configuration.GetSection("WebhookOptions"));
        services.AddScoped<IWebhookUrlFactory, WebhookUrlFactory>();

        #endregion

        #region Shared.Infrastructure

        services.Configure<DatabaseContextOptions>(configuration.GetSection("DatabaseContext"));
        services.AddScoped<DatabaseContext>();

        #endregion

        #region GitLab.Infrastructure

        services.AddScoped<GitLab.PlatformRepository>();

        #endregion
    }
}