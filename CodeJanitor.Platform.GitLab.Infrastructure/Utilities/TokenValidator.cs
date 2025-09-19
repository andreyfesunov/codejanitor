using CodeJanitor.Platform.Domain.Exceptions;
using GitLabApiClient;
using GitLabApiClient.Models;

namespace CodeJanitor.Platform.GitLab.Infrastructure.Utilities;

public class TokenValidator(string url, string token)
{
    private readonly GitLabClient _client = new(BuildHost(url), token);

    public async Task ValidateTokenAndPermissionsAsync()
    {
        try
        {
            var uri = new Uri(url);
            var path = uri.AbsolutePath.TrimStart('/').TrimEnd('/');

            var project = await _client.Projects.GetAsync(path);
            var permissions = project.Permissions.ProjectAccess;

            if (permissions == null || permissions.AccessLevel < (int)AccessLevel.Reporter)
                throw new UnauthorizedTokenException("Not enough permissions to clone repository.");

            if (permissions.AccessLevel < (int)AccessLevel.Developer)
                throw new UnauthorizedTokenException("Not enough permissions to create branches and merge requests.");
        }
        catch (Exception e)
        {
            throw new UnauthorizedTokenException("Token is not valid.", e);
        }
    }

    private static string BuildHost(string url)
    {
        var uri = new Uri(url);
        return uri.Scheme + "://" + uri.Host;
    }
}