using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Projects.Commands
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project(Guid.NewGuid(), request.Name, request.Description, request.UserId);

            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                TaskCount = 0,
                CompletedTaskCount = 0,
                PendingTaskCount = 0
            };
        }
    }
}