using MediatR;
using TaskManager.Application.Interfaces;

namespace TaskManager.Application.Tasks.Commands
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTaskCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);

            if (task == null)
                return false;

            _unitOfWork.Tasks.Delete(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}