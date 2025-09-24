using CodeJanitor.DependencyRules.Tests.Utilities;
using NUnit.Framework;

namespace CodeJanitor.DependencyRules.Tests.Application;

[TestFixture]
public sealed class DockerApplicationProjectTests
{
    private const string Project = "CodeJanitor.Docker.Application";

    [Test]
    public void ApplicationShouldNotDependOnAnyExceptDomain()
    {
        var matcher = ProjectReferenceChecker.SubstringListMatcher(
            ["Application", "Infrastructure", "Web"],
            Project
        );
        var matchingRefs = ProjectReferenceChecker.GetMatchingReferences(Project, matcher);
        Assert.That(matchingRefs, Is.Empty);
    }
}