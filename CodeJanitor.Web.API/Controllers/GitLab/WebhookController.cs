using CodeJanitor.Web.API.Mappers;
using CodeJanitor.Web.API.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeJanitor.Web.API.Controllers.GitLab;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class WebhookController(IMediator mediator) : ControllerBase
{
    [HttpPost("gitlab")]
    public async Task<Unit> CreateGitLabIssue(
        [FromBody] CreateIssueRequestModel request,
        [FromQuery] Guid repositoryId
    )
    {
        var command = WebhookMapper.Map(request, repositoryId);
        return await mediator.Send(command);
    }

    [HttpPost("docker")]
    public async Task<Unit> DeleteDockerData([FromBody] DeleteDockerDataRequestModel request)
    {
        var command = WebhookMapper.Map(request);
        return await mediator.Send(command);
    }
}