using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.Tasks.Commands
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CommentResponse> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new DomainException("Task not found");

            var comment = task.AddComment(request.Content, request.UserId);

            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CommentResponse
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserId = comment.UserId
            };
        }
    }
}