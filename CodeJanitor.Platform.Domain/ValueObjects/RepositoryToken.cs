using System.Text.RegularExpressions;
using CodeJanitor.Platform.Domain.Exceptions;
using CodeJanitor.Platform.Domain.Models;

namespace CodeJanitor.Platform.Domain.ValueObjects;

public sealed record RepositoryToken
{
    private const string GitLabFormat = "^glpat-[a-zA-Z0-9-]{19}$";

    public readonly string Value;

    private RepositoryToken(string value)
    {
        Value = value;
    }

    public static RepositoryToken Create(string raw, Provider provider)
    {
        return provider switch
        {
            Provider.GitLab => CreateGitLab(raw.Trim()),
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };
    }

    private static RepositoryToken CreateGitLab(string raw)
    {
        if (!Regex.IsMatch(raw, GitLabFormat))
            throw new InvalidPlatformTokenException(Provider.GitLab);

        return new RepositoryToken(raw);
    }

    public Provider GetProvider()
    {
        return Value switch
        {
            not null when Regex.IsMatch(Value, GitLabFormat) => Provider.GitLab,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}