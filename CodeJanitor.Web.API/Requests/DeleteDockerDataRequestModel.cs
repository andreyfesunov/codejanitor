namespace CodeJanitor.Web.API.Requests;

public sealed record DeleteDockerDataRequestModel
{
    public required string ContainerId { get; init; }
}