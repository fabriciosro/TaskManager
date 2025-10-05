using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Projects.Queries
{
    public class GetUserProjectsQuery : IRequest<List<ProjectResponse>>
    {
        public Guid UserId { get; set; }

        public GetUserProjectsQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}