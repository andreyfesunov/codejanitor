namespace CodeJanitor.Platform.Domain.Models;

public sealed class Workflow(
    string content,
    IEnumerable<string> instructions,
    IDictionary<string, string> secrets
)
{
    public readonly string Content = content;
    public readonly IEnumerable<string> Instructions = instructions;
    public readonly IDictionary<string, string> Secrets = secrets;
}