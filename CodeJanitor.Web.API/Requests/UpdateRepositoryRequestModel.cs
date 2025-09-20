namespace CodeJanitor.Web.API.Requests;

public sealed record UpdateRepositoryRequestModel
{
    public required string RepositoryUrl { get; init; }
    public required string PreviousAccessToken { get; init; }
    public required string NewAccessToken { get; init; }
}