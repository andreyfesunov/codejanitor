namespace CodeJanitor.Web.API.Requests;

public sealed record RegisterRepositoryRequestModel
{
    public required string RepositoryUrl { get; init; }
    public required string AccessToken { get; init; }
}