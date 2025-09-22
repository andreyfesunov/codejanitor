namespace CodeJanitor.Web.API.Requests;

public sealed record CreateIssueRequestModel
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required IEnumerable<string> Labels { get; init; }
}