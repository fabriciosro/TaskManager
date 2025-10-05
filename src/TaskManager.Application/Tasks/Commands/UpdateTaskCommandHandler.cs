using MediatR;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Application.Tasks.Commands
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTaskCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskResponse> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new DomainException("Task not found");

            if (request.Title != null)
                task.UpdateTitle(request.Title);

            if (request.Description != null)
                task.UpdateDescription(request.Description);

            if (request.DueDate.HasValue)
                task.UpdateDueDate(request.DueDate.Value);

            if (request.Status.HasValue)
                task.ChangeStatus(request.Status.Value, request.UserId);

            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return MapToTaskResponse(task);
        }

        private TaskResponse MapToTaskResponse(Domain.Entities.ProjectTask task)
        {
            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                History = task.History.Select(h => new TaskHistoryResponse
                {
                    FieldChanged = h.FieldChanged,
                    OldValue = h.OldValue,
                    NewValue = h.NewValue,
                    ChangedAt = h.ChangedAt,
                    ChangedByUserId = h.ChangedByUserId
                }).ToList(),
                Comments = task.Comments.Select(c => new CommentResponse
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserId
                }).ToList()
            };
        }
    }
}