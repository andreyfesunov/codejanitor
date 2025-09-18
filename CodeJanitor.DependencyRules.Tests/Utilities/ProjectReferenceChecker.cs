using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CodeJanitor.DependencyRules.Tests.Utilities;

public static class ProjectReferenceChecker
{
    /// <summary>
    ///     Finds the solution root directory by traversing up until a .sln file is found.
    /// </summary>
    /// <returns>The full path to the solution root.</returns>
    /// <exception cref="FileNotFoundException">Thrown if no .sln file is found.</exception>
    private static string FindSolutionRoot()
    {
        var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (currentDir != null)
        {
            if (Directory.EnumerateFiles(currentDir.FullName, "*.sln").Any()) return currentDir.FullName;
            currentDir = currentDir.Parent;
        }

        throw new FileNotFoundException("Solution root (.sln file) not found.");
    }

    /// <summary>
    ///     Builds the .csproj path for the given project name, assuming it's in a subfolder under the solution root.
    /// </summary>
    /// <param name="projectName">The name of the project (e.g., 'CodeJanitor.Platform.Domain').</param>
    /// <returns>The full path to the .csproj file.</returns>
    private static string BuildCsprojPath(string projectName)
    {
        var solutionRoot = FindSolutionRoot();
        return Path.Combine(solutionRoot, projectName, $"{projectName}.csproj");
    }

    /// <summary>
    ///     Retrieves project references from the specified .csproj file that match the provided criteria.
    /// </summary>
    /// <param name="projectName">Full path to the .csproj file.</param>
    /// <param name="matcher">A function that takes a reference 'Include' value and returns true if it matches the criteria.</param>
    /// <returns>List of matching reference paths; empty if none found.</returns>
    public static IEnumerable<string> GetMatchingReferences(string projectName, Func<string, bool> matcher)
    {
        var csprojPath = BuildCsprojPath(projectName);
        if (!File.Exists(csprojPath)) throw new FileNotFoundException($"Project file not found: {csprojPath}");

        var doc = XDocument.Load(csprojPath);
        var msbuild = doc.Root?.GetDefaultNamespace() ??
                      throw new InvalidOperationException("MSBuild namespace not found");

        var references = doc.Descendants(msbuild + "ProjectReference")
            .Select(refElem => refElem.Attribute("Include")?.Value)
            .Where(include => !string.IsNullOrEmpty(include))
            .OfType<string>();

        return references.Where(matcher);
    }

    public static Func<string, bool> SubstringMatcher(string substring, string? exclude = null)
    {
        return include => include.Contains(substring) && (exclude == null || !include.Contains(exclude));
    }

    public static Func<string, bool> SubstringListMatcher(IEnumerable<string> substrings, string? exclude = null)
    {
        return include =>
            substrings.Any(include.Contains) && (exclude == null || !include.Contains(exclude));
    }

    public static Func<string, bool> RegexMatcher(string regexPattern, string? exclude = null)
    {
        var regex = new Regex(regexPattern);
        return include => regex.IsMatch(include) && (exclude == null || !include.Contains(exclude));
    }

    public static Func<string, bool> RegexListMatcher(IEnumerable<string> regexPatterns, string? exclude = null)
    {
        var regexes = regexPatterns.Select(p => new Regex(p)).ToList();
        return include => regexes.Any(r => r.IsMatch(include)) && (exclude == null || !include.Contains(exclude));
    }
}