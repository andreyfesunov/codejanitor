using CodeJanitor.Web.API.Models;

namespace CodeJanitor.Web.API.Responses;

public sealed record RegisterRepositoryResponseModel
{
    public required Guid Id { get; init; }
    public required WorkflowModel Workflow { get; init; }
}