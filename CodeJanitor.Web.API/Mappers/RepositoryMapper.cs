using CodeJanitor.Platform.Application.Commands;
using CodeJanitor.Platform.Application.Responses;
using CodeJanitor.Web.API.Requests;
using CodeJanitor.Web.API.Responses;

namespace CodeJanitor.Web.API.Mappers;

public static class RepositoryMapper
{
    public static RegisterRepositoryCommand Map(RegisterRepositoryRequestModel requestModel)
    {
        return new RegisterRepositoryCommand
        {
            RepositoryUrl = requestModel.RepositoryUrl,
            AccessToken = requestModel.AccessToken
        };
    }

    public static RegisterRepositoryResponseModel Map(RegisterRepositoryResponse response)
    {
        return new RegisterRepositoryResponseModel
        {
            Id = response.Repository.Id,
            Workflow = WorkflowMapper.Map(response.Workflow)
        };
    }
}