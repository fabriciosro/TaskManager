using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;

namespace TaskManager.Application.Projects.Queries
{
    public class GetUserProjectsQueryHandler : IRequestHandler<GetUserProjectsQuery, List<ProjectResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserProjectsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ProjectResponse>> Handle(GetUserProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _unitOfWork.Projects.GetUserProjectsAsync(request.UserId);

            return projects.Select(p => new ProjectResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                TaskCount = p.Tasks.Count,
                CompletedTaskCount = p.GetCompletedTasksCount(),
                PendingTaskCount = p.Tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Pending)
            }).ToList();
        }
    }
}