using CodeJanitor.Platform.Application;
using CodeJanitor.Platform.Domain.Factories;
using CodeJanitor.Web.API.Factories;
using GitLab = CodeJanitor.Platform.GitLab.Infrastructure.Repositories;

namespace CodeJanitor.Web.API;

public static class Startup
{
    public static void Configure(this IServiceCollection services)
    {
        #region Controllers (API)

        services.AddControllers();

        #endregion

        #region MediatR

        services.AddMediatR(config =>
            config.RegisterServicesFromAssemblies(typeof(PlatformApplicationMarker).Assembly));

        #endregion

        #region Factories

        services.AddScoped<IPlatformRepositoryFactory, PlatformRepositoryFactory>();

        #endregion

        #region GitLab.Infrastructure

        services.AddScoped<GitLab.PlatformRepository, GitLab.PlatformRepository>();

        #endregion
    }
}