using CodeJanitor.Docker.Application;
using CodeJanitor.Docker.Domain.Repositories;
using CodeJanitor.Docker.Infrastructure.Repositories;
using CodeJanitor.Platform.Application;
using CodeJanitor.Platform.Domain.Factories;
using CodeJanitor.Shared.Infrastructure.Contexts;
using CodeJanitor.Web.API.Factories;
using GitLabRepositories = CodeJanitor.Platform.GitLab.Infrastructure.Repositories;
using GitLabFactories = CodeJanitor.Platform.GitLab.Infrastructure.Factories;
using DockerFactories = CodeJanitor.Docker.Infrastructure.Factories;

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
        services.AddScoped<GitLabFactories.IWebhookUrlFactory, WebhookUrlFactory>();
        services.AddScoped<DockerFactories.IWebhookUrlFactory, WebhookUrlFactory>();

        services.Configure<DockerOptions>(configuration.GetSection("DockerOptions"));
        services.AddScoped<DockerFactories.IDockerFactory, DockerFactory>();

        #endregion

        #region Docker.Infrastructure

        services.AddScoped<IDockerRepository, DockerRepository>();

        #endregion

        #region GitLab.Infrastructure

        services.AddScoped<GitLabRepositories.PlatformRepository>();

        #endregion

        #region Shared.Infrastructure

        services.Configure<DatabaseContextOptions>(configuration.GetSection("DatabaseContext"));
        services.AddScoped<DatabaseContext>();

        #endregion
    }
}