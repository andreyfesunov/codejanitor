using CodeJanitor.DependencyRules.Tests.Utilities;
using NUnit.Framework;

namespace CodeJanitor.DependencyRules.Tests.Domain;

[TestFixture]
public sealed class SharedDomainProjectTests
{
    private const string Project = "CodeJanitor.Shared.Domain";

    [Test]
    public void DomainShouldNotDependOnAnyOtherInternalAssembly()
    {
        var matcher = ProjectReferenceChecker.SubstringListMatcher(
            ["Domain", "Application", "Infrastructure", "Web"],
            Project
        );
        var matchingRefs = ProjectReferenceChecker.GetMatchingReferences(Project, matcher);
        Assert.That(matchingRefs, Is.Empty);
    }
}