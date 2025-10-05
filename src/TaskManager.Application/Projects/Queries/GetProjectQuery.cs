using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Projects.Queries
{
    public class GetProjectQuery : IRequest<ProjectResponse>
    {
        public Guid ProjectId { get; set; }

        public GetProjectQuery(Guid projectId)
        {
            ProjectId = projectId;
        }
    }
}