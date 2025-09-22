using CodeJanitor.Web.API.Mappers;
using CodeJanitor.Web.API.Requests;
using CodeJanitor.Web.API.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeJanitor.Web.API.Controllers.GitLab;

[ApiController]
[Route("api/v1/gitlab/[controller]")]
public class RepositoryController(ISender mediator) : ControllerBase
{
    [HttpPost]
    public async Task<RegisterRepositoryResponseModel> Register([FromBody] RegisterRepositoryRequestModel requestModel)
    {
        var command = RepositoryMapper.Map(requestModel);
        var result = await mediator.Send(command);
        return RepositoryMapper.Map(result);
    }

    [HttpPut]
    public async Task<UpdateRepositoryResponseModel> Update([FromBody] UpdateRepositoryRequestModel requestModel)
    {
        var command = RepositoryMapper.Map(requestModel);
        var result = await mediator.Send(command);
        return RepositoryMapper.Map(result);
    }
}