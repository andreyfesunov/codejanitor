using CodeJanitor.DependencyRules.Tests.Utilities;
using NUnit.Framework;

namespace CodeJanitor.DependencyRules.Tests.Application;

[TestFixture]
public sealed class PlatformApplicationProjectTests
{
    private const string Project = "CodeJanitor.Platform.Application";

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