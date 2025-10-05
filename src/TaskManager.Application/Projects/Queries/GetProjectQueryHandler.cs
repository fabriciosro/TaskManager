using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.Projects.Queries
{
    public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, ProjectResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProjectQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectResponse> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId);

            if (project == null)
                throw new DomainException("Project not found");

            return new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                TaskCount = project.Tasks.Count,
                CompletedTaskCount = project.GetCompletedTasksCount(),
                PendingTaskCount = project.Tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Pending)
            };
        }
    }
}