using CodeJanitor.Platform.Application.Commands;
using CodeJanitor.Web.API.Requests;

namespace CodeJanitor.Web.API.Mappers;

public static class WebhookMapper
{
    public static ProcessIssueCommand Map(CreateIssueRequestModel request)
    {
        return new ProcessIssueCommand
        {
            Labels = request.Labels,
            Title = request.Title,
            Description = request.Description
        };
    }
}