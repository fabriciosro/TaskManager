using MediatR;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.Projects.Commands
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProjectCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId);

            if (project == null || project.UserId != request.UserId)
                return false;

            if (project.HasPendingTasks())
                throw new DomainException("Cannot delete project with pending tasks. Complete or remove tasks first.");

            _unitOfWork.Projects.Delete(project);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}