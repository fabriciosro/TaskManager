using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Tasks.Queries
{
    public class GetProjectTasksQuery : IRequest<List<TaskResponse>>
    {
        public Guid ProjectId { get; set; }

        public GetProjectTasksQuery(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}