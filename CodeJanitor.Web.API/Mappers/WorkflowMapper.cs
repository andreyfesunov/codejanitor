using CodeJanitor.Platform.Domain.Models;
using CodeJanitor.Web.API.Models;

namespace CodeJanitor.Web.API.Mappers;

public static class WorkflowMapper
{
    public static WorkflowModel Map(Workflow workflow)
    {
        return new WorkflowModel();
    }
}