using CodeJanitor.Platform.Domain.Models;
using CodeJanitor.Platform.Domain.Repositories;
using CodeJanitor.Platform.Domain.ValueObjects;
using CodeJanitor.Platform.GitLab.Infrastructure.Factories;
using CodeJanitor.Platform.GitLab.Infrastructure.Utilities;
using CodeJanitor.Shared.Infrastructure.Contexts;
using Dapper;

namespace CodeJanitor.Platform.GitLab.Infrastructure.Repositories;

public sealed class PlatformRepository(
    DatabaseContext context,
    IWebhookUrlFactory webhookUrlFactory
) : IPlatformRepository
{
    public async Task<Repository?> GetByUrlAsync(RepositoryUrl url)
    {
        return await context.ExecuteAsync(async connection =>
        {
            var result = await connection.QueryAsync<Repository>(
                "SELECT * FROM repositories WHERE url = @Url",
                new { Url = url.Value }
            );

            return result.FirstOrDefault();
        });
    }

    public async Task<Repository> RequireByIdAsync(Guid id)
    {
        return await context.ExecuteAsync(async connection =>
        {
            var result = await connection.QueryAsync<Repository>(
                "SELECT * FROM repositories WHERE id = @Id",
                new { Id = id }
            );

            return result.First();
        });
    }

    public async Task<Repository> CreateAsync(Repository repository)
    {
        return await context.ExecuteAsync(async connection =>
        {
            await connection.ExecuteAsync(
                "INSERT INTO repositories (id, url, token) VALUES (@Id, @Url, @Token)",
                new { repository.Id, Url = repository.Url.Value, Token = repository.Token.Value }
            );

            return repository;
        });
    }

    public async Task<Repository> UpdateAsync(Repository repository)
    {
        return await context.ExecuteAsync(async connection =>
        {
            await connection.ExecuteAsync(
                "UPDATE repositories SET url = @Url, token = @Token WHERE id = @Id",
                new { repository.Id, Url = repository.Url.Value, Token = repository.Token.Value }
            );

            return repository;
        });
    }

    public async Task ValidateAsync(Repository repository)
    {
        var validator = new TokenValidator(repository.Url.Value.ToString(), repository.Token.Value);

        await validator.ValidateTokenAndPermissionsAsync();
    }

    public async Task<Workflow> GetWorkflowAsync(Repository repository)
    {
        var workflow = new Workflow(
            GetWorkflowContent(),
            GetWorkflowInstructions(),
            await GetWorkflowSecrets(webhookUrlFactory, repository.Id)
        );

        return workflow;
    }

    private static string GetWorkflowContent()
    {
        return """
               stages:
                 - notify

               notify_codejanitor:
                 stage: notify
                 rules:
                   - if: '$CI_PIPELINE_SOURCE == "issue"'
                     when: on_success
                 script:
                   - |
                     ISSUE_ACTION=$(curl -s --header "PRIVATE-TOKEN: $GITLAB_TOKEN" "https://gitlab.com/api/v4/projects/$CI_PROJECT_ID/issues/$CI_ISSUE_IID" | jq -r '.action')
                     if [ "$ISSUE_ACTION" != "open" ]; then
                       echo "Skipping: not a new issue creation"
                       exit 0
                     fi
                     PAYLOAD=$(jq -n \
                       --arg title "$CI_ISSUE_TITLE" \
                       --arg description "$CI_ISSUE_DESCRIPTION" \
                       --argjson labels "$CI_ISSUE_LABELS" \
                       '{issue: {title: $title, description: $description, labels: $labels}}')
                     curl -X POST "$CI_CODEJANITOR_WEBHOOK_URL" \
                       -H "Content-Type: application/json" \
                       -d "$PAYLOAD" \
                       --fail --silent --show-error
                     echo "Webhook sent to CodeJanitor successfully"
               """;
    }

    private static IEnumerable<string> GetWorkflowInstructions()
    {
        return
        [
            "1. Copy the workflow YAML content above into a new file named `.gitlab-ci.yml` at the root of your repository.",
            "2. Replace any placeholder secrets (such as `CI_CODEJANITOR_WEBHOOK_URL`) with your actual values in your GitLab project's CI/CD variables.",
            "3. Ensure that the `GITLAB_TOKEN` variable is set up in your project's CI/CD variables with a valid GitLab access token.",
            "4. Commit and push the `.gitlab-ci.yml` file to your repository.",
            "5. The workflow will trigger automatically when a new issue is created in your GitLab project."
        ];
    }

    private static async Task<IDictionary<string, string>> GetWorkflowSecrets(IWebhookUrlFactory factory,
        Guid repositoryId)
    {
        return new Dictionary<string, string>([
            new KeyValuePair<string, string>("CI_CODEJANITOR_WEBHOOK_URL", await factory.CreateGitLabUrl(repositoryId))
        ]);
    }
}