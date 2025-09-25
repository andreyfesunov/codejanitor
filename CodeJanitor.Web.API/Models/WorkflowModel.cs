namespace CodeJanitor.Web.API.Models;

public sealed record WorkflowModel
{
    public required string Content { get; init; }
    public required IEnumerable<string> Instructions { get; init; }
    public required IDictionary<string, string> Secrets { get; init; }
}