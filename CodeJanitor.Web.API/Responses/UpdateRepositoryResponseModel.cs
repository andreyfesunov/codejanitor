namespace CodeJanitor.Web.API.Responses;

public sealed record UpdateRepositoryResponseModel
{
    public required Guid Id { get; init; }
}