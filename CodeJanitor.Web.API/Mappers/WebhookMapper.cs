using CodeJanitor.Docker.Application.Commands;
using CodeJanitor.Platform.Application.Commands;
using CodeJanitor.Web.API.Requests;

namespace CodeJanitor.Web.API.Mappers;

public static class WebhookMapper
{
    public static ProcessIssueCommand Map(CreateIssueRequestModel request, Guid repositoryId)
    {
        return new ProcessIssueCommand
        {
            RepositoryId = repositoryId,
            Labels = request.Labels,
            Title = request.Title,
            Description = request.Description
        };
    }

    public static DeleteDockerDataCommand Map(DeleteDockerDataRequestModel request)
    {
        return new DeleteDockerDataCommand
        {
            ContainerId = request.ContainerId
        };
    }
}